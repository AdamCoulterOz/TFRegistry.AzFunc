using PurpleDepot.Interface.Model;

namespace PurpleDepot.Interface.Storage;
public interface IStorageProvider<T>
	where T : RegistryItem<T>
{
	public abstract Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(string fileKey);
	public abstract Task<bool> UploadZipAsync(string fileKey, Stream stream);
	public abstract Uri DownloadLink(string fileKey);
}
