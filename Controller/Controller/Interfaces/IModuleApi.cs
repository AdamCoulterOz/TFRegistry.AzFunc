using PurpleDepot.Interface.Model.Module;

namespace PurpleDepot.Controller.Interfaces;

public interface IModuleApi
{
	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Download"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Download(IHttpRequest request, string @namespace, string name, string provider);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.DownloadVersion"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> DownloadVersion(IHttpRequest request, string @namespace, string name, string provider, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Post"/>
	/// Route: <see cref="ModuleRoutes.Ingest"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Ingest(IHttpRequest request, string @namespace, string name, string provider, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Latest"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Latest(IHttpRequest request, string @namespace, string name, string provider);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Specific"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Version(IHttpRequest request, string @namespace, string name, string provider, string version);

	/// <summary>
	/// Method: <see cref="HttpMethod.Get"/>
	/// Route: <see cref="ModuleRoutes.Versions"/>
	/// Request: <see cref="IHttpRequest"/>
	/// Response: <see cref="IHttpResponse"/>
	/// </summary>
	Task<IHttpResponse> Versions(IHttpRequest request, string @namespace, string name, string provider);
}
