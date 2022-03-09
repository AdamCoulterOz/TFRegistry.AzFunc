using System.Net;
using Controller;

namespace Controller.Exceptions;
public class HttpResponseException : Exception
{
	public HttpResponseMessage Response { get; }

	public HttpResponseException(HttpRequestMessage requestMessage, HttpStatusCode statusCode, string message = "")
		: base(message)
	{
		Response = requestMessage.CreateStringResponse(statusCode, message);
	}
}
