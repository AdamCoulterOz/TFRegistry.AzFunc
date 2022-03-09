using PurpleDepot.Core.Controller.Exceptions;
using Microsoft.Azure.Functions.Worker.Http;

namespace PurpleDepot.Providers.Azure;

public static class HttpMessageShims
{
	private static HttpRequestMessage AsRequestMessage(this HttpRequestData request)
	{
		var newMessage = new HttpRequestMessage(new HttpMethod(request.Method), request.Url)
		{
			Content = new StreamContent(request.Body)
		};
		var nonContentHeaders = request.Headers.ToList().Where(header => !header.Key.Contains("Content"));
		foreach (var (key, value) in nonContentHeaders)
		{
			newMessage.Headers.Add(key, value);
		}
		var contentHeaders = request.Headers.ToList().Where(header => header.Key.Contains("Content"));
		foreach (var (key, value) in contentHeaders)
		{
			newMessage.Content.Headers.Add(key, value);
		}
		return newMessage;
	}

	private static HttpResponseData AsResponseData(this HttpResponseMessage response, HttpRequestData request)
	{
		var newData = request.CreateResponse(response.StatusCode);
		newData.Body = response.Content.ReadAsStream();
		var headers = response.Headers.ToList();
		headers.AddRange(response.Content.Headers.ToList());
		newData.Headers = new HttpHeadersCollection(headers);
		return newData;
	}

	public static async Task<HttpResponseData> ShimHttp(this HttpRequestData request, Func<HttpRequestMessage, Task<HttpResponseMessage>> controllerFunc)
	{
		try
		{
			return (await controllerFunc(request.AsRequestMessage())).AsResponseData(request);
		}
		catch (HttpResponseException re)
		{
			return re.Response.AsResponseData(request);
		}
	}
}
