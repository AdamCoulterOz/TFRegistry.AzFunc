using System;
using System.Net;
using System.Net.Http;

namespace PurpleDepot.Controller
{
	public static class AuthController
	{
		public static void Authenticate(this HttpRequestMessage request)
		{
			var authHeader = request.Headers.Authorization;
			if (authHeader is null)
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, "Authorization header hasn't been provided, and is required.");
			if(authHeader.Scheme != "Bearer")
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, $"Incorrected authorisation type provided, expected 'bearer' got '{authHeader.Scheme}'");
			throw new HttpResponseException(request, HttpStatusCode.Unauthorized, authHeader.Parameter);
		}
	}
}