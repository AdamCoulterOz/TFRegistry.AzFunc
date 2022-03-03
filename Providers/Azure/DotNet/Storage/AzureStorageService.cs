using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PurpleDepot.Interface.Storage;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Options;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Providers.Azure.Storage;
public class AzureStorageService<T> : IStorageProvider<T>
	where T : RegistryItem
{
	private readonly AzureStorageOptions _options;

	public AzureStorageService(IOptions<AzureStorageOptions> options)
		=> _options = options.Value;

	private BlobClient GetBlobClient(string fileKey)
	{
		var blobName = fileKey;
		var containerName = typeof(T).Name;
		NameValidator.ValidateContainerName(containerName);
		NameValidator.ValidateBlobName(blobName);

		var credOptions = new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true };
		var credential = new DefaultAzureCredential(credOptions);
		var containerClient = new BlobContainerClient(_options.BlobClientUrl, credential);
		var blobClient = containerClient.GetBlobClient(blobName);
		return blobClient;
	}

	public Uri DownloadLink(string fileKey) => GetBlobClient(fileKey).Uri;

	public async Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(string fileKey)
	{
		var client = GetBlobClient(fileKey);
		var exists = await client.ExistsAsync();
		if (!exists) return (null, null);
		BlobDownloadInfo download = await client.DownloadAsync();
		return (download.Content, download.ContentLength);
	}

	public async Task<bool> UploadZipAsync(string fileKey, Stream stream)
	{
		var client = GetBlobClient(fileKey);
		var header = new BlobHttpHeaders { ContentType = "application/zip" };
		await client.UploadAsync(content: stream, httpHeaders: header);
		stream.Close();

		var properties = (await client.GetPropertiesAsync()).Value;
		if (properties.ContentLength >= 0)
			return true;

		await client.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);
		return false;
	}
}
