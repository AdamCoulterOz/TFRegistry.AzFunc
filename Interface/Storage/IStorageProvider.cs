using System;
using System.IO;
using System.Threading.Tasks;

namespace PurpleDepot.Interface.Storage
{
	public interface IStorageProvider
	{
		public abstract Stream? DownloadZip(Guid fileKey);
		public abstract Task UploadZip(Guid fileKey, Stream stream);
	}
}