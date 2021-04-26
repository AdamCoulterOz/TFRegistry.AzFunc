using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;

namespace PurpleDepot.Controller
{
	public static class ControllerExtensions
	{
		public static HttpResponseData CreateSerializedResponse(this HttpRequestData request, object document)
		{
			var body = JsonSerializer.Serialize(document);
			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "application/json");
			response.WriteString(body);
			return response;
		}
	}
}