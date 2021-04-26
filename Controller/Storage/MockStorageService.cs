using System;
using System.IO;

namespace PurpleDepot.Controller.Storage
{
	public class MockStorageService : IStorageProvider
	{
		public Stream DownloadFile()
		{
			var file = "asfhjklasfghjkalsfd";
			return new MemoryStream(Convert.ToByte(file));
		}
	}
}