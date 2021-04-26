using System.IO;

namespace PurpleDepot.Controller.Storage
{
	public interface IStorageProvider
	{
		public abstract Stream DownloadFile();
	}
}