using Microsoft.Azure.Functions.Worker.Http;
using PurpleDepot.Controller;

namespace PurpleDepot.Providers.Azure;

public static class HttpMessageShims
{
	public static HttpRequestMessage AsRequestMessage(this HttpRequestData request)
	{
		var newMessage = new HttpRequestMessage(new HttpMethod(request.Method), request.Url)
		{
			Content = new StreamContent(request.Body)
		};
		var nonContentHeaders = request.Headers.ToList().Where(header => !header.Key.Contains("Content"));
		foreach (var nonContentheader in nonContentHeaders)
		{
			newMessage.Headers.Add(nonContentheader.Key, nonContentheader.Value);
		}
		var contentHeaders = request.Headers.ToList().Where(header => header.Key.Contains("Content"));
		foreach (var contentHeader in contentHeaders)
		{
			newMessage.Content.Headers.Add(contentHeader.Key, contentHeader.Value);
		}
		return newMessage;
	}

	public static HttpResponseData AsResponseData(this HttpResponseMessage response, HttpRequestData request)
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
