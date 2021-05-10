using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Threading.Tasks;
using PurpleDepot.Controller;
using PurpleDepot.Data;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Providers.Azure
{
	public class ModuleAPI : ModuleController
	{
		public ModuleAPI(ModuleContext moduleContext, IStorageProvider storageProvider)
			: base(moduleContext, storageProvider) { 
				moduleContext.Database.EnsureDeleted();
				moduleContext.Database.EnsureCreated();
			}

		[Function(nameof(GetVersions))]
		public async Task<HttpResponseData> GetVersions(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/versions")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			try
			{
				return (await Versions(
						request.AsRequestMessage(),
						@namespace, name, provider)
					).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetDownload))]
		public async Task<HttpResponseData> GetDownload(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			try
			{
				return (await Download(
						request.AsRequestMessage(),
						@namespace, name, provider)
					).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetDownloadSpecific))]
		public async Task<HttpResponseData> GetDownloadSpecific(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			try
			{
				return (await DownloadSpecific(
						request.AsRequestMessage(),
						@namespace, name, provider, version)
					).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetLatest))]
		public async Task<HttpResponseData> GetLatest(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			try
			{
				return (await Latest(
						request.AsRequestMessage(),
						@namespace, name, provider)
					).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetSpecific))]
		public async Task<HttpResponseData> GetSpecific(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			try
			{
				return (await Specific(
						request.AsRequestMessage(),
						@namespace, name, provider, version)
					).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(PostIngest))]
		public async Task<HttpResponseData> PostIngest(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/modules/{namespace}/{name}/{provider}/{version}/upload")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			try
			{
				return (await Ingest(
						request.AsRequestMessage(),
						@namespace, name, provider, version)
					).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}
	}
}
