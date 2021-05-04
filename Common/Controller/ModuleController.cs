using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using PurpleDepot.Model;
using PurpleDepot.Data;
using System.Threading.Tasks;
using PurpleDepot.Controller.Storage;

namespace PurpleDepot.Controller
{
	public class ModuleController
	{
		private readonly ModuleContext _moduleContext;
		private readonly IStorageProvider _storageProvider;
		public ModuleController(ModuleContext moduleContext, IStorageProvider storageProvider)
		{
			_moduleContext = moduleContext;
			_storageProvider = storageProvider;
		}

		[Function(nameof(Versions))]
		public async Task<HttpResponseData> Versions(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/versions")]
			HttpRequestData request, FunctionContext executionContext,
			string @namespace, string name, string provider)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);

			var logger = executionContext.GetLogger(nameof(Versions));
			logger.LogInformation("");

			var module = await _moduleContext.GetModule(@namespace, name, provider);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var moduleCollection = new ModuleCollection();
			moduleCollection.Modules.Add(module);
			return request.CreateSerializedResponse(moduleCollection);
		}

		[Function(nameof(Download))]
		public async Task<HttpResponseData> Download(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			return await DownloadSpecific(request, @namespace, name, provider, "latest");
		}

		[Function(nameof(DownloadSpecific))]
		public async Task<HttpResponseData> DownloadSpecific(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}/download")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);

			var module = await _moduleContext.GetModule(@namespace, name, provider, version);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);
			var downloadStream = _storageProvider.DownloadZip(module.FileKey);
			if(downloadStream is null)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Body = downloadStream;
			response.Headers.Add("Content-Type", "application/zip");
			response.Headers.Add("Content-Disposition", new string[] { "attachment", $"filename={module.FileName(version)}" });
			return response;
		}

		[Function(nameof(Latest))]
		public async Task<HttpResponseData> Latest(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}")]
			HttpRequestData request,
			string @namespace, string name, string provider)
		{
			return await Specific(request, @namespace, name, provider, "latest");
		}

		[Function(nameof(Specific))]
		public async Task<HttpResponseData> Specific(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/modules/{namespace}/{name}/{provider}/{version}")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			if (!AuthController.Authenticated(request.Headers))
				return request.CreateResponse(HttpStatusCode.Unauthorized);
			var module = await _moduleContext.GetModule(@namespace, name, provider, version);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);
			return request.CreateSerializedResponse(module);
		}

		[Function(nameof(Ingest))]
		public async Task<HttpResponseData> Ingest(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/modules/{namespace}/{name}/{provider}/{version}/upload")]
			HttpRequestData request,
			string @namespace, string name, string provider, string version)
		{
			var module = await _moduleContext.GetModule(@namespace, name, provider, version);
			if (module is not null)
				return request.CreateResponse(HttpStatusCode.Conflict);
			var length = request.Headers.GetValues("Content-Length").FirstOrDefault();
			if (length is null || "".Equals(length))
				return request.CreateResponse(HttpStatusCode.BadRequest);
			
			module = new Module(@namespace, name, provider, version);
			
			using var stream = request.Body;
			await _storageProvider.UploadZip(module.FileKey, stream);

			_moduleContext.Add(module);
			_moduleContext.SaveChanges();

			return request.CreateResponse(HttpStatusCode.Created);
		}
	}
}

