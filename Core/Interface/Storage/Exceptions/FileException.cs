namespace PurpleDepot.Core.Interface.Storage.Exceptions;

public abstract class FileException : Exception
{
	public string FileKey { get; set; }
	protected FileException(string fileKey, string message, Exception? innerException = null)
	: base($"File with key {fileKey} {message}", innerException)
		=> FileKey = fileKey;
}
