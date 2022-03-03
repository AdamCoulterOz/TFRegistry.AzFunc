using System.Net;
namespace PurpleDepot.Controller.Exceptions;
public class HttpResponseException : Exception
{
	public HttpResponseMessage Response { get; init; }
	public HttpResponseException(HttpRequestMessage requestMessage, HttpStatusCode statusCode, string message = "")
		: base(message: message)
	{
		Response = requestMessage.CreateStringResponse(statusCode, message);
	}

	public HttpResponseException(HttpResponseMessage Response, string message = "")
		: base(message: message)
	{
		this.Response = Response;
	}
}
