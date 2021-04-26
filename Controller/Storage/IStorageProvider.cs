using System;
using System.IO;
using System.Threading.Tasks;

namespace PurpleDepot.Controller.Storage
{
	public interface IStorageProvider
	{
		public abstract Stream? DownloadFile(Guid fileKey);
		public abstract Task UploadFile(Guid fileKey, Stream stream);
	}
}