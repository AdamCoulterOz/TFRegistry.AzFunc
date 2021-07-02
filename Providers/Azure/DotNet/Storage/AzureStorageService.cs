using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PurpleDepot.Interface.Storage;
using Microsoft.Azure.Storage;
using Microsoft.Extensions.Options;

namespace PurpleDepot.Providers.Azure.Storage
{
	public class AzureStorageService : IStorageProvider
	{
		private readonly AzureStorageOptions _options;

		public AzureStorageService(IOptions<AzureStorageOptions> options)
			=> _options = options.Value;

		private BlobClient GetBlobClient(Guid fileKey, ObjectType type)
		{
			var blobName = fileKey.ToString();
			var containerName = Enum.GetName(type);
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

		public Uri DownloadLink(Guid fileKey, ObjectType type) => GetBlobClient(fileKey, type).Uri;

		public async Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(Guid fileKey, ObjectType type)
		{
			var client = GetBlobClient(fileKey, type);
			var exists = await client.ExistsAsync();
			if (!exists) return (null, null);
			BlobDownloadInfo download = await client.DownloadAsync();
			return (download.Content, download.ContentLength);
		}

		public async Task<bool> UploadZipAsync(Guid fileKey, Stream stream, ObjectType type)
		{
			var client = GetBlobClient(fileKey, type);
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
}