using Interface.Model.Provider;

namespace Controller.Controller.Interfaces;

public interface IProviderApi
{
	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Download"/>
	/// </summary>
	Task<HttpResponseMessage> Download(HttpRequestMessage request, string @namespace, string name);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.DownloadVersion"/>
	/// </summary>
	Task<HttpResponseMessage> DownloadVersion(HttpRequestMessage request, string @namespace, string name, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Post"/>
	/// Route: <see cref="ProviderRoutes.Ingest"/>
	/// </summary>
	Task<HttpResponseMessage> Ingest(HttpRequestMessage request, string @namespace, string name, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Latest"/>
	/// </summary>
	Task<HttpResponseMessage> Latest(HttpRequestMessage request, string @namespace, string name);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Version"/>
	/// </summary>
	Task<HttpResponseMessage> Version(HttpRequestMessage request, string @namespace, string name, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ProviderRoutes.Versions"/>
	/// </summary>
	Task<HttpResponseMessage> Versions(HttpRequestMessage request, string @namespace, string name);
}
