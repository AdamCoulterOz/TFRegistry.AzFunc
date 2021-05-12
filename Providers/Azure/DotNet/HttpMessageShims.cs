using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Azure.Functions.Worker.Http;

namespace PurpleDepot.Providers.Azure
{
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

		public static Uri BaseUrl(this HttpRequestData request)
		{
			return new UriBuilder
			{
				Scheme = request.Url.Scheme,
				Host = request.Url.Host,
				Port = request.Url.Port
			}.Uri;
		}
	}
}