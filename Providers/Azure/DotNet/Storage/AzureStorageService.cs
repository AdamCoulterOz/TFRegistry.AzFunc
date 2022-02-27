using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PurpleDepot.Interface.Storage;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Options;

namespace PurpleDepot.Providers.Azure.Storage;
public class AzureStorageService : IStorageProvider
{
	private readonly AzureStorageOptions _options;

	public AzureStorageService(IOptions<AzureStorageOptions> options)
		=> _options = options.Value;

	private BlobClient GetBlobClient<T>(string fileKey)
	{
		var blobName = fileKey;
		var containerName = typeof(T).Name;
		NameValidator.ValidateContainerName(containerName);
		NameValidator.ValidateBlobName(blobName);
		var options = new DefaultAzureCredentialOptions()
		{
			ExcludeSharedTokenCacheCredential = true
		};
		var credential = new DefaultAzureCredential(options);
		var uri = new Uri(_options.BlobEndpointUrl, containerName);
		var containerClient = new BlobContainerClient(uri, credential);
		var blobClient = containerClient.GetBlobClient(blobName);
		return blobClient;
	}

	Uri IStorageProvider.DownloadLink<T>(string fileKey) => GetBlobClient<T>(fileKey).Uri;

	async Task<(Stream? Stream, long? ContentLength)> IStorageProvider.DownloadZipAsync<T>(string fileKey)
	{
		var client = GetBlobClient<T>(fileKey);
		var exists = await client.ExistsAsync();
		if (!exists) return (null, null);
		BlobDownloadInfo download = await client.DownloadAsync();
		return (download.Content, download.ContentLength);
	}

	async Task<bool> IStorageProvider.UploadZipAsync<T>(string fileKey, Stream stream)
	{
		var client = GetBlobClient<T>(fileKey);
		var header = new BlobHttpHeaders()
		{
			ContentType = "application/zip"
		};
		await client.UploadAsync(content: stream, httpHeaders: header);
		stream.Close();

		BlobProperties properties = await client.GetPropertiesAsync();
		if (properties.ContentLength >= 0L) return true;

		await client.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
		return false;
	}
}
