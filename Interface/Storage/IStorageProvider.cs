using Interface.Model;

namespace Interface.Storage;
public interface IStorageProvider<T>
	where T : RegistryItem<T>
{
	public Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(string fileKey);
	public Task<bool> UploadZipAsync(string fileKey, Stream stream);
	public Uri DownloadLink(string fileKey);
}
