using System.Text;
using Interface.Model;
using Interface.Storage.Exceptions;

namespace Interface.Storage;
public class MockStorageService<T> : IStorageProvider<T> where T: RegistryItem<T>
{
	private static readonly Dictionary<string, string> _files = new();
	public Uri DownloadLink(string fileKey)
	{
		throw new NotImplementedException();
	}

	public async Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(string fileKey)
	{
		if (!_files.ContainsKey(fileKey))
			throw new Exceptions.FileNotFound(fileKey);

		return await Task.Run(() =>
		{
			var bytes = Encoding.ASCII.GetBytes(_files[fileKey]);
			return (new MemoryStream(bytes), bytes.Length);
		});
	}

	public async Task UploadZipAsync(string fileKey, Stream stream)
	{
		if(_files.ContainsKey(fileKey))
			throw new FileAlreadyExists(fileKey);
		using var sr = new StreamReader(stream);
		_files.Add(fileKey, await sr.ReadToEndAsync());
	}
}
