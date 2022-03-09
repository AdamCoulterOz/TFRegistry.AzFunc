using System.Net;
using System.Net.Http.Json;

namespace Controller;
public static class HttpResponseExtensions
{
	public static HttpResponseMessage CreateResponse(this HttpRequestMessage request, HttpStatusCode statusCode)
	{
		return new HttpResponseMessage(statusCode)
		{
			RequestMessage = request
		};
	}
	public static HttpResponseMessage CreateStringResponse(this HttpRequestMessage requestMessage, HttpStatusCode statusCode, string message)
	{
		var response = requestMessage.CreateResponse(statusCode);
		response.Content = new StringContent(message);
		return response;
	}
	public static HttpResponseMessage CreateJsonResponse(this HttpRequestMessage request, object document)
	{
		return new HttpResponseMessage(HttpStatusCode.OK)
		{
			RequestMessage = request,
			Content = JsonContent.Create(document)
		};
	}
}
