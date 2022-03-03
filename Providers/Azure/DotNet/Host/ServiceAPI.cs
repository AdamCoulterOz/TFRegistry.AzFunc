using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using PurpleDepot.Controller;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Providers.Azure.Host;

public class ServiceApi : ServiceController //IServiceApi
{
	[Function(nameof(ServiceDiscoveryAsync))]
	public async Task<HttpResponseData> ServiceDiscoveryAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ServiceRoutes.WellKnownUrl)] HttpRequestData request)
			=> await request.ShimHttp(async (req) => await ServiceDiscovery(req));
}
