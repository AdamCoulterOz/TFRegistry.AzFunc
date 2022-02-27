using System.Net;

namespace PurpleDepot.Interface.Exceptions;
public class AlreadyExistsException : Exception
{
	public static HttpStatusCode HttpStatusCode
		=> HttpStatusCode.Conflict;
	public string RequestType { get; }
	public AlreadyExistsException(object obj) : base()
		=> RequestType = obj.GetType().Name;
	public override string Message
		=> $"Cannot create new {RequestType} as one already exists";
}
