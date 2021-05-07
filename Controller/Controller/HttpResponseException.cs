using System;
using System.Net;
using System.Net.Http;

namespace PurpleDepot.Controller
{
	public class HttpResponseException : Exception
	{
		public HttpResponseMessage Response { get; init; }
		public HttpResponseException(HttpStatusCode statusCode, string message = "") 
			: base(message: message)
		{
			Response = new HttpResponseMessage(statusCode);
		}

		public HttpResponseException(HttpResponseMessage Response, string message = "") 
			: base(message: message)
		{
			this.Response = Response;
		}
	}
}