using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using PurpleDepot.Controller;

namespace PurpleDepot.Providers.Azure
{
	public class ServiceAPI : ServiceController
	{
		[Function(nameof(ServiceDiscovery))]
		public static HttpResponseData ServiceDiscovery(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".well-known/terraform.json")] HttpRequestData request)
		{
			try
			{
				return ServiceDiscovery(request.AsRequestMessage(), "/v1/modules").AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}
	}
}