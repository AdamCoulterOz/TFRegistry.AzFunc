using PurpleDepot.Interface.Model;

namespace PurpleDepot.Interface.Storage;
public enum ObjectType { Module, Provider }
public interface IStorageProvider<T> where T : IRegistryItem
{
	public abstract Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(string fileKey);
	public abstract Task<bool> UploadZipAsync(string fileKey, Stream stream);
	public abstract Uri DownloadLink(string fileKey);
}
