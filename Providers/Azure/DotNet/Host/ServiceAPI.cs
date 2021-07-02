using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using PurpleDepot.Controller;

namespace PurpleDepot.Providers.Azure.Host
{
	public class ServiceAPI : ServiceController
	{
		[Function(nameof(ServiceDiscovery))]
		public static HttpResponseData ServiceDiscovery(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".well-known/terraform.json")] HttpRequestData request)
		{
			try
			{
				return ServiceDiscovery(request.AsRequestMessage()).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}
	}
}