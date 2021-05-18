using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PurpleDepot.Interface.Storage
{
	public class MockStorageService : IStorageProvider
	{
		public async Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(Guid fileKey)
		{
			return await Task.Run(() =>
			{
				var file = "asfhjklasfghjkalsfd";
				var bytes = Encoding.ASCII.GetBytes(file);

				return (new MemoryStream(bytes), bytes.Length);
			});
		}

		public async Task<bool> UploadZipAsync(Guid fileKey, Stream stream)
		{
			using var sr = new StreamReader(stream);
			await sr.ReadToEndAsync();
			return true;
		}
	}
}