using PurpleDepot.Interface.Model.Provider;

namespace PurpleDepot.Controller.Interfaces;

public interface IProviderApi
{
	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Download"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Download(IHttpRequest request, string @namespace, string name);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.DownloadVersion"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> DownloadVersion(IHttpRequest request, string @namespace, string name, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Post"/>
	/// Route: <see cref="ProviderRoutes.Ingest"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Ingest(IHttpRequest request, string @namespace, string name, string provider, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Latest"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Latest(IHttpRequest request, string @namespace, string name);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Specific"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Version(IHttpRequest request, string @namespace, string name, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Versions"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Versions(IHttpRequest request, string @namespace, string name);
}
