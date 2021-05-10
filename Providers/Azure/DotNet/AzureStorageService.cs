using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using PurpleDepot.Interface.Configuration;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Providers.Storage.Azure
{
	public class AzureStorageService : IStorageProvider
	{
		[EnvironmentVariable("STORAGE_ACCOUNT", required: true)]
		private string StorageAccount { get; set; }

		[EnvironmentVariable("BLOB_CONTAINER", required: true)]
		private string BlobContainer { get; set; }

		public AzureStorageService()
		{
			this.InitializeAttributes();
		}

		private BlobClient GetBlobClient(Guid fileKey)
		{
			var options = new DefaultAzureCredentialOptions()
			{
				ExcludeSharedTokenCacheCredential = true
			};
			var credential = new DefaultAzureCredential(options);
			var uri = new Uri($"https://{StorageAccount}.blob.core.windows.net/{BlobContainer}/");
			var containerClient = new BlobContainerClient(uri, credential);
			var blobClient = containerClient.GetBlobClient(fileKey.ToString());
			return blobClient;
		}

		public Stream? DownloadZip(Guid fileKey)
		{
			var client = GetBlobClient(fileKey);
			return client.Download().GetRawResponse().ContentStream;
		}

		public async Task UploadZip(Guid fileKey, Stream stream)
		{
			var client = GetBlobClient(fileKey);

			var header = new BlobHttpHeaders()
			{
				ContentType = "application/zip"
			};
			await client.UploadAsync(content: stream, httpHeaders: header);
		}
	}
}