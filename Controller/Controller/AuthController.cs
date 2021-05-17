using System.Net;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace PurpleDepot.Controller
{
	public static class AuthController
	{
		public static void Authenticate(this HttpRequestMessage request)
		{
			var authHeader = request.Headers.Authorization;
			if (authHeader is null)
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, "Authorization header hasn't been provided, and is required.");
			if (authHeader.Scheme != "Bearer")
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, $"Incorrected authorisation type provided, expected 'bearer' got '{authHeader.Scheme}'");

			var tokenHandler = new JwtSecurityTokenHandler();
			var rawToken = authHeader.Parameter;

			var readableToken = tokenHandler.CanReadToken(rawToken);

			if (readableToken != true)
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, "The token doesn't seem to be in a proper JWT format.");

			var token = tokenHandler.ReadJwtToken(rawToken);
			var roles = token.Claims.Where(claim => claim.Type == "roles").First().Value;

			if(!roles.Contains("Terraform.Module.Contributor"))
				throw new HttpResponseException(request, HttpStatusCode.Unauthorized, $"The token doesn't doesn't contain the module contributor role, found roles: {roles}");
		}
	}
}