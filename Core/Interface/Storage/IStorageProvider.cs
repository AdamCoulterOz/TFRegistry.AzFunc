using PurpleDepot.Core.Interface.Model;
using PurpleDepot.Core.Interface.Storage.Exceptions;

namespace PurpleDepot.Core.Interface.Storage;
public interface IStorageProvider<T>
	where T : RegistryItem<T>
{
	/// <exception cref="FileNotFound">File doesnt exist with key</exception>
	public Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(string fileKey);

	/// <exception cref="FileAlreadyExists">If a file already exists with key</exception>
	/// <exception cref="FileEmpty">Upload stream has no content</exception>
	public Task UploadZipAsync(string fileKey, Stream stream);

	/// <exception cref="FileNotFound">File doesnt exist with key</exception>
	public Uri DownloadLink(string fileKey);
}
