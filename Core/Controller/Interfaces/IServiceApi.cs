using Interface.Model;

namespace Controller.Controller.Interfaces;

public interface IServiceApi
{
	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ServiceRoutes.WellKnownUrl"/>
	/// </summary>
	Task<HttpResponseMessage> ServiceDiscoveryAsync(HttpRequestMessage request);
}
