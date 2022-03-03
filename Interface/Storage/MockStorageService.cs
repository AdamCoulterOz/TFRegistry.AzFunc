using System.Text;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Interface.Storage;
public class MockStorageService<T> : IStorageProvider<T> where T: RegistryItem<T>
{
	public Uri DownloadLink(string fileKey)
	{
		throw new NotImplementedException();
	}

	public async Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(string fileKey)
	{
		return await Task.Run(() =>
		{
			var file = "asfhjklasfghjkalsfd";
			var bytes = Encoding.ASCII.GetBytes(file);

			return (new MemoryStream(bytes), bytes.Length);
		});
	}

	public async Task<bool> UploadZipAsync(string fileKey, Stream stream)
	{
		using var sr = new StreamReader(stream);
		await sr.ReadToEndAsync();
		return true;
	}
}
