namespace PurpleDepot.Core.Interface.Storage.Exceptions;

public class FileNotFound : FileException
{
	public FileNotFound(string fileKey, Exception? innerException = null)
		: base(fileKey, "not found", innerException) { }
}
