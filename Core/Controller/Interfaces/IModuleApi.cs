using PurpleDepot.Core.Interface.Model.Module;

namespace PurpleDepot.Core.Controller.Controller.Interfaces;

public interface IModuleApi
{
	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Download"/>
	/// </summary>
	Task<HttpResponseMessage> Download(HttpRequestMessage request, string @namespace, string name, string provider);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.DownloadVersion"/>
	/// </summary>
	Task<HttpResponseMessage> DownloadVersion(HttpRequestMessage request, string @namespace, string name, string provider, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Post"/>
	/// Route: <see cref="ModuleRoutes.Ingest"/>
	/// </summary>
	Task<HttpResponseMessage> Ingest(HttpRequestMessage request, string @namespace, string name, string provider, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Latest"/>
	/// </summary>
	Task<HttpResponseMessage> Latest(HttpRequestMessage request, string @namespace, string name, string provider);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Version"/>
	/// </summary>
	Task<HttpResponseMessage> Version(HttpRequestMessage request, string @namespace, string name, string provider, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Versions"/>
	/// </summary>
	Task<HttpResponseMessage> Versions(HttpRequestMessage request, string @namespace, string name, string provider);
}
