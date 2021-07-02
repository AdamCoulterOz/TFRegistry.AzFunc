using System;
using System.IO;
using System.Threading.Tasks;

namespace PurpleDepot.Interface.Storage
{
	public enum ObjectType { Module, Provider }
	public interface IStorageProvider
	{
		public abstract Task<(Stream? Stream, long? ContentLength)> DownloadZipAsync(Guid fileKey, ObjectType type);
		public abstract Task<bool> UploadZipAsync(Guid fileKey, Stream stream, ObjectType type);
		public abstract Uri DownloadLink(Guid fileKey, ObjectType type);
	}
}