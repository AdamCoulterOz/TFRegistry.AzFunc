namespace Interface.Storage.Exceptions;

public class FileAlreadyExists : FileException
{
	public FileAlreadyExists(string fileKey, Exception? innerException = null)
		: base(fileKey, "already exists", innerException) { }
}
