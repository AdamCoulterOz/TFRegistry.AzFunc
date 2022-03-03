namespace Azure.Storage;

public class AzureStorageOptions
{
	public Uri BlobClientUrl => new($"https://{StorageAccountName}.blob.core.windows.net/{BlobContainerName}");
	public string StorageAccountName { get; init; } = null!;
	public string BlobContainerName { get; init; } = null!;
}
