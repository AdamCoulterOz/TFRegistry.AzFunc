using System.Net;

namespace PurpleDepot.Core.Controller.Exceptions;
public class HttpResponseException : Exception
{
	public ControllerResult Response { get; }

	public HttpResponseException(HttpStatusCode statusCode, string message = "")
		: base(message) => Response = ControllerResult.New(statusCode, message);
}
