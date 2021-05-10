using System;
using System.Net;
using System.Net.Http;

namespace PurpleDepot.Controller
{
	public static class AuthController
	{
		public static void Authenticate(this HttpRequestMessage request)
		{
			var token = Environment.GetEnvironmentVariable("PURPLEDEPOT_AUTHTOKEN");
			if (token is null)
				throw new HttpResponseException(request, HttpStatusCode.InternalServerError, "Authentication is not configured.");
			var authHeader = request.Headers.Authorization;
			if (authHeader is null)
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, "Authorization header hasn't been provided, and is required.");
			if(authHeader.Scheme != "Bearer")
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, $"Incorrected authorisation type provided, expected 'bearer' got '{authHeader.Scheme}'");
			if(authHeader.Parameter != token)
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, "Bearer token invalid.");
		}
	}
}