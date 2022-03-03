namespace PurpleDepot.Providers.Azure.Storage;

public class AzureStorageOptions
{
	public Uri BlobClientUrl => new Uri($"https://{StorageAccountName}.blob.core.windows.net/{BlobContainerName}");
	public string StorageAccountName { get; set; } = null!;
	public string BlobContainerName { get; set; } = null!;
}
