using Controller.Controller;
using Interface.Model;
using Interface.Model.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Azure.Host;

public class ServiceApi : ServiceController //IServiceApi
{
	[Function(nameof(ServiceDiscoveryAsync))]
	public async Task<HttpResponseData> ServiceDiscoveryAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ServiceRoutes.WellKnownUrl)] HttpRequestData request)
			=> await request.ShimHttp(async req => await ServiceDiscovery(req));
}
