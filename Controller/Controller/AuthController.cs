using System.Net;
using System.Net.Http;

namespace PurpleDepot.Controller
{
	public static class AuthController
	{
		public static void Authenticate(this HttpRequestMessage request)
		{
			//request.Headers.Authorization
			throw new HttpResponseException(request.CreateResponse(HttpStatusCode.Unauthorized));
		}
	}
}