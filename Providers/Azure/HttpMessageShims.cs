using System;
using System.Net.Http;
using System.Linq;
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
			request.Headers.ToList().ForEach(header => newMessage.Headers.Add(header.Key, header.Value));
			return newMessage;
		}

		public static HttpResponseData AsResponseData(this HttpResponseMessage response, HttpRequestData request)
		{	
			var newData = request.CreateResponse(response.StatusCode);
			newData.Body = response.Content.ReadAsStream();
			newData.Headers = new HttpHeadersCollection(response.Headers.ToList());
			return newData;
		}
	}
}