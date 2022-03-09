namespace Interface.Storage.Exceptions;
public class FileEmpty : FileException
{
	public FileEmpty(string fileKey, Exception? innerException = null)
		: base(fileKey, "stream was empty", innerException) { }
}
