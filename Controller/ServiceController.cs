using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace PurpleDepot.Controller
{
	public class ServiceController
	{
		[Function(nameof(ServiceDiscovery))]
		public static HttpResponseData ServiceDiscovery(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".well-known/terraform.json")]
			HttpRequestData request,
			FunctionContext executionContext)
		{
			var logger = executionContext.GetLogger(nameof(ServiceDiscovery));
			logger.LogInformation($"{nameof(ServiceDiscovery)} invoked");

			var services = new Dictionary<string, string>()
			{
				["modules.v1"] = "/v1/modules"
			};

			var body = JsonSerializer.Serialize(services);
			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "application/json");
			response.WriteString(body);
			return response;
		}
	}
}