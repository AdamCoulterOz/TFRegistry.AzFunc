using PurpleDepot.Interface.Model;

namespace PurpleDepot.Controller.Interfaces;

public interface IServiceApi
{
	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ServiceRoutes.WellKnownUrl"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> ServiceDiscoveryAsync(IHttpRequest request);
}
