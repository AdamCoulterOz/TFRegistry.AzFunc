using System;
using System.IO;
using System.Threading.Tasks;

namespace PurpleDepot.Interface.Storage
{
	public interface IStorageProvider
	{
		public abstract Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(Guid fileKey);
		public abstract Task UploadZipAsync(Guid fileKey, Stream stream);
	}
}