using PurpleDepot.Core.Controller;
using PurpleDepot.Core.Interface.Model.Service;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace PurpleDepot.Providers.Azure.Host;

public class ServiceApi : ServiceController //IServiceApi
{
	[Function(nameof(ServiceDiscoveryAsync))]
	public async Task<HttpResponseData> ServiceDiscoveryAsync(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ServiceRoutes.WellKnownUrl)] HttpRequestData request)
			=> await request.CreateResponseAsync(async () => await ServiceDiscovery());
}
