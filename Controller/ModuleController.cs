using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Identity;
using System;
using System.Linq;
using Azure.Storage.Blobs.Models;
using System.Management.Automation;
using System.IO;
using System.Text;
using PurpleDepot.Model;

namespace PurpleDepot.Controller
{
	public static class ModuleController
	{
		[Function(nameof(Versions))]
		public static HttpResponseData Versions(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/versions")]
			HttpRequestData request, FunctionContext executionContext,
			string @namespace, string name, string provider)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);

			var logger = executionContext.GetLogger(nameof(Versions));
			logger.LogInformation("");

			var doc = new ModuleCollection();
			var module = new Module();

			doc.Modules.Add(module);

			string name1;
			if(module.Name is not null)
				name1 = module.Name;

			var container = ContainerClient;
			var prefixItems = container.GetBlobsByHierarchy(prefix: GetPrefix(@namespace, provider, name), delimiter: "/")
					.Where(item => item.IsPrefix).ToList();
			foreach (var item in prefixItems)
			{
				var version = SemanticVersion.Parse(item.Prefix);
				module.Versions.Add(new VersionElement(version));
			}

			var body = JsonSerializer.Serialize(doc);
			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "application/json");
			response.WriteString(body);
			return response;
		}

		[Function(nameof(Download))]
		public static HttpResponseData Download(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);

			var latest = GetLatestVersion(@namespace, name, provider);
			if (latest is null)
				return request.CreateResponse(HttpStatusCode.NotFound);
			var file = GetModuleClient(provider, @namespace, name, latest);
			if (!file.Exists())
				throw new Exception("Lost file");

			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Body = file.OpenRead();
			response.Headers.Add("Content-Type", "application/zip");
			response.Headers.Add("Content-Disposition", new string[] { "attachment", $"filename={GetFileName(provider, @namespace, name, latest)}" });
			return response;
		}

		[Function(nameof(DownloadSpecific))]
		public static HttpResponseData DownloadSpecific(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);

			var file = GetModuleClient(provider, @namespace, name, version);
			if (!file.Exists())
				return request.CreateResponse(HttpStatusCode.NotFound);

			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Body = file.OpenRead();
			response.Headers.Add("Content-Type", "application/zip");
			response.Headers.Add("Content-Disposition", new string[] { "attachment", $"filename={GetFileName(provider, @namespace, name, version)}" });
			return response;
		}

		[Function(nameof(Latest))]
		public static HttpResponseData Latest(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);

			var latest = GetLatestVersion(@namespace, name, provider);
			if (latest is null)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var module = new Module()
			{
				Name = name,
				Namespace = @namespace,
				Provider = provider,
				Version = SemanticVersion.Parse(latest)
			};

			var response = request.CreateResponse(HttpStatusCode.OK);
			var jsonModule = JsonSerializer.Serialize(module);
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModule));
			response.Body = stream;
			response.Headers.Add("Content-Type","application/json");
			return response;
		}

		[Function(nameof(Specific))]
		public static HttpResponseData Specific(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);

			var file = GetModuleClient(provider, @namespace, name, version);
			if(!file.Exists().Value)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var module = new Module()
			{
				Name = name,
				Namespace = @namespace,
				Provider = provider,
				Version = SemanticVersion.Parse(version)
			};

			var response = request.CreateResponse(HttpStatusCode.OK);
			var jsonModule = JsonSerializer.Serialize(module);
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonModule));
			response.Body = stream;
			response.Headers.Add("Content-Type","application/json");
			return response;
		}

		[Function(nameof(Ingest))]
		public static HttpResponseData Ingest(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/modules/{namespace}/{name}/{provider}/{version}/upload")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			var file = GetModuleClient(provider, @namespace, name, version);
			if (file.Exists().Value)
				return request.CreateResponse(HttpStatusCode.Conflict);
			var length = request.Headers.GetValues("Content-Length").FirstOrDefault();
			if (length is null || "".Equals(length))
				return request.CreateResponse(HttpStatusCode.BadRequest);
			using var stream = request.Body;
			var header = new BlobHttpHeaders()
			{
				ContentType = "application/zip"
			};
			file.Upload(content: stream, httpHeaders: header);
			return request.CreateResponse(HttpStatusCode.Created);
		}



		private static BlobContainerClient ContainerClient
		{
			get
			{
				var options = new DefaultAzureCredentialOptions(){
					ExcludeSharedTokenCacheCredential = true
				};
				var credential = new DefaultAzureCredential(options);
				var uri = new Uri($"https://{Configuration.StorageAccount}.blob.core.windows.net/{Configuration.BlobContainer}/");
				return new BlobContainerClient(uri, credential);
			}
		}

		private static string GetPrefix(string @namespace, string provider, string name, string? version = null)
		{
			var prefix = $"{@namespace}/{provider}/{name}/";
			if(version is not null)
				prefix += $"{version}/";
			return prefix;
		}

		private static string? GetLatestVersion(string @namespace, string provider, string name)
		{
			return ContainerClient.GetBlobsByHierarchy(prefix: GetPrefix(@namespace, provider, name), delimiter: "/")
					.Where(item => item.IsPrefix).ToList()
					.OrderBy(item => item.Prefix).LastOrDefault()?.Prefix;
		}

		private static BlobClient GetModuleClient(string provider, string @namespace, string name, string version)
		{
			string fileName = GetFileName(provider, @namespace, name, version);
			string prefix = GetPrefix(@namespace, provider, name, version);
			
			string path = $"{prefix}{fileName}";

			var containerClient = ContainerClient;
			var blobClient = containerClient.GetBlobClient(path);
			return blobClient;
		}

		private static string GetFileName(string provider, string @namespace, string name, string version)
		{
			return $"{@namespace}-{provider}-{name}-{version}.zip";
		}
	}
}

