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
			: base(moduleContext, storageProvider) { }

		[Function(nameof(GetVersions))]
		public HttpResponseData GetVersions(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/versions")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			try
			{
				return Versions(request.AsRequestMessage(), @namespace, name, provider).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetDownloadAsync))]
		public async Task<HttpResponseData> GetDownloadAsync(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			try
			{
				return (await DownloadAsync(request.AsRequestMessage(), @namespace, name, provider)).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetDownloadSpecificAsync))]
		public async Task<HttpResponseData> GetDownloadSpecificAsync(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			try
			{
				return (await DownloadSpecificAsync(request.AsRequestMessage(), @namespace, name, provider, version)).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetLatest))]
		public HttpResponseData GetLatest(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			try
			{
				return Latest(request.AsRequestMessage(), @namespace, name, provider).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetSpecific))]
		public HttpResponseData GetSpecific(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			try
			{
				if(version == "versions")
					return GetVersions(request, @namespace, name, provider);
				return Specific(request.AsRequestMessage(), @namespace, name, provider, version).AsResponseData(request);
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
				return (await Ingest(request.AsRequestMessage(), @namespace, name, provider, version)).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}
	}
}
