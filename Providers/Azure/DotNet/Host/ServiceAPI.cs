using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using PurpleDepot.Controller;

namespace PurpleDepot.Providers.Azure.Host;
public class ServiceApi : ServiceController
{
	[Function(nameof(ServiceDiscoveryAsync))]
	public static async Task<HttpResponseData> ServiceDiscoveryAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ".well-known/terraform.json")] HttpRequestData request)
			=> await request.ShimHttp(async (req) => await ServiceDiscovery(req));
}
