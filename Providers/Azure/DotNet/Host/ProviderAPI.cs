using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Mvc;
using PurpleDepot.Interface.Storage;
using System.IO;
using PurpleDepot.Interface.Host;

namespace PurpleDepot.Providers.Azure.Host
{
	public class ProviderAPI : ProviderController
	{
		public ProviderAPI(ProviderContext moduleContext, IStorageProvider storageProvider)
			: base(moduleContext, storageProvider) { }

		[Function(nameof(GetDownloadProviderAsync))]
		public async Task<HttpResponseData> GetDownloadProviderAsync(
			[HttpTrigger("get", Route = "download/{fileKey}/{fileName}")]
			HttpRequestData request, string fileKey, string fileName)
		{
			try
			{
				return (await DownloadProviderAsync(request.AsRequestMessage(), new Guid(fileKey), fileName)).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetVersions))]
		public HttpResponseData GetVersions(
			[HttpTrigger("get", Route = "v1/providers/{namespace}/{name}/versions")]
			HttpRequestData request, string @namespace, string name)
		{
			try
			{
				return Versions(request.AsRequestMessage(), @namespace, name).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetDownload))]
		public HttpResponseData GetDownload(
			[HttpTrigger("get", Route = "v1/providers/{namespace}/{name}/download")]
			HttpRequestData request, string @namespace, string name)
		{
			try
			{
				return Download(request.AsRequestMessage(), @namespace, name).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetDownloadVersion))]
		public HttpResponseData GetDownloadVersion(
			[HttpTrigger("get", Route = "v1/providers/{namespace}/{name}/{version}/download")]
			HttpRequestData request,
			string @namespace, string name, string version)
		{
			try
			{
				return DownloadSpecific(request.AsRequestMessage(), @namespace, name, version).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetLatest))]
		public HttpResponseData GetLatest(
			[HttpTrigger("get", Route = "v1/providers/{namespace}/{name}")]
			HttpRequestData request,
			string @namespace, string name)
		{
			try
			{
				return Latest(request.AsRequestMessage(), @namespace, name).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(GetSpecific))]
		public HttpResponseData GetSpecific(
			[HttpTrigger("get", Route = "v1/providers/{namespace}/{name}/{version}")]
			HttpRequestData request,
			string @namespace, string name, string version)
		{
			try
			{
				if (version == "versions")
					return GetVersions(request, @namespace, name);
				return Specific(request.AsRequestMessage(), @namespace, name, version).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}

		[Function(nameof(PostIngest))]
		public async Task<HttpResponseData> PostIngest(
			[HttpTrigger("post", Route = ProviderRoutes.PostIngest)]
			 [FromQuery] Provider providerRequest, [FromBody] Stream stream )
		{
			stream.SetLength(5L);
			try
			{
				return (await Ingest(request.AsRequestMessage(), @namespace, name, version)).AsResponseData(request);
			}
			catch (HttpResponseException re)
			{
				return re.Response.AsResponseData(request);
			}
		}
	}
}
