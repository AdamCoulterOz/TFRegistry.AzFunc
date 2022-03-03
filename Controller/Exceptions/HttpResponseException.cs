using System.Net;
using Controller.Controller;

namespace Controller.Exceptions;
public abstract class HttpResponseException : Exception
{
	public HttpResponseMessage Response { get; }

	protected HttpResponseException(HttpRequestMessage requestMessage, HttpStatusCode statusCode, string message = "")
		: base(message)
	{
		Response = requestMessage.CreateStringResponse(statusCode, message);
	}
}
