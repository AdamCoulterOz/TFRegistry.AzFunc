using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using PurpleDepot.Controller;
using PurpleDepot.Data;
using PurpleDepot.Interface.Model.Module;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Providers.Azure.Host;
public class ModuleApi : ModuleController
{
	public ModuleApi(IRepository<Module> repo, IStorageProvider<Module> storageProvider) : base(repo, storageProvider) { }

	[Function($"{nameof(ModuleApi)}_{nameof(Versions)}")]
	public async Task<HttpResponseData> Versions(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ModuleRoutes.Versions)]
			HttpRequestData request, string @namespace, string name, string provider)
				=> await request.ShimHttp(async (req) => await VersionsAsync(req, new ModuleAddress(@namespace, name, provider)));

	[Function($"{nameof(ModuleApi)}_{nameof(Download)}")]
	public async Task<HttpResponseData> Download(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ModuleRoutes.Download)]
			HttpRequestData request, string @namespace, string name, string provider)
				=> await request.ShimHttp(async (req) => await DownloadAsync(req, new ModuleAddress(@namespace, name, provider)));

	[Function($"{nameof(ModuleApi)}_{nameof(DownloadVersion)}")]
	public async Task<HttpResponseData> DownloadVersion(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ModuleRoutes.DownloadVersion)]
			HttpRequestData request,
		string @namespace, string name, string provider, string version)
			=> await request.ShimHttp(async (req) => await DownloadVersionAsync(req, new ModuleAddress(@namespace, name, provider), version));

	[Function($"{nameof(ModuleApi)}_{nameof(Latest)}")]
	public async Task<HttpResponseData> Latest(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ModuleRoutes.Latest)]
			HttpRequestData request,
		string @namespace, string name, string provider)
			=> await request.ShimHttp(async (req) => await LatestAsync(req, new ModuleAddress(@namespace, name, provider)));

	[Function($"{nameof(ModuleApi)}_{nameof(Version)}")]
	public async Task<HttpResponseData> Version(
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ModuleRoutes.Version)]
			HttpRequestData request,
		string @namespace, string name, string provider, string version)
	{
		if (version == "versions")
			return await Versions(request, @namespace, name, provider);
		return await request.ShimHttp(async (req) => await VersionAsync(req, new ModuleAddress(@namespace, name, provider), version));
	}

	[Function($"{nameof(ModuleApi)}_{nameof(Ingest)}")]
	public async Task<HttpResponseData> Ingest(
		[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ModuleRoutes.Ingest)]
			HttpRequestData request,
		string @namespace, string name, string provider, string version)
			=> await request.ShimHttp(async (req) => await IngestAsync(req, Module.New(new ModuleAddress(@namespace, name, provider), version), request.Body));
}
