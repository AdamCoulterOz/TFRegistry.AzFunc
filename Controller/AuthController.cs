using Microsoft.Azure.Functions.Worker.Http;

namespace PurpleDepot.Controller
{
	public class AuthController
	{
		public static bool Authenticated(HttpHeadersCollection headers)
		{
			// TODO: Add actual token verification
			return headers.Contains("Authorization");
		}
	}
}