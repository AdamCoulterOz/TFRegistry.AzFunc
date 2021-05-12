using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PurpleDepot.Data;
using PurpleDepot.Model;
using PurpleDepot.Interface.Storage;
using Microsoft.Net.Http.Headers;

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
			_moduleContext.Database.EnsureCreated();
		}

		public HttpResponseMessage Versions(
			HttpRequestMessage request,
			string @namespace, string name, string provider)
		{
			request.Authenticate();

			var module = _moduleContext.GetModule(@namespace, name, provider);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var moduleCollection = new ModuleCollection();
			moduleCollection.Modules.Add(module);
			return request.CreateJsonResponse(moduleCollection);
		}

		public async Task<HttpResponseMessage> DownloadAsync(
			HttpRequestMessage request, string @namespace, string name, string provider)
		{
			return await DownloadSpecificAsync(request, @namespace, name, provider, "latest");
		}

		public async Task<HttpResponseMessage> DownloadSpecificAsync(
			HttpRequestMessage request, string @namespace, string name, string provider, string version)
		{
			request.Authenticate();
			var module = _moduleContext.GetModule(@namespace, name, provider);
			if (module is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module doesn't exist at any version.");
			if(!module.HasVersion(version))
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module found, but specified version doesn't exist.");
			var fileKey = module.FileId(version);
			if(fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "Couldn't get file key.");
			var (blobDownloadStream, blobContentLength) = await _storageProvider.DownloadZipAsync(fileKey.Value);
			if (blobDownloadStream is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module found at required version, but cannot find module zip.");
			return request.CreateZipDownloadResponse(blobDownloadStream, module.FileName(version), blobContentLength);
		}

		public HttpResponseMessage Latest(
			HttpRequestMessage request,
			string @namespace, string name, string provider)
		{
			request.Authenticate();
			return Specific(request, @namespace, name, provider, "latest");
		}

		public HttpResponseMessage Specific(
			HttpRequestMessage request,
			string @namespace, string name, string provider, string version)
		{
			request.Authenticate();
			var module = _moduleContext.GetModule(@namespace, name, provider);
			if (module is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module doesn't exist at any version.");
			if (!module.HasVersion(version))
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module doesn't exist at specified version.");
			return request.CreateJsonResponse(module);
		}

		public async Task<HttpResponseMessage> Ingest(
			HttpRequestMessage request,
			string @namespace, string name, string provider, string version)
		{
			request.Authenticate();

			if (request.Content is null)
				return request.CreateResponse(HttpStatusCode.BadRequest);

			var module = _moduleContext.GetModule(@namespace, name, provider);
			var hasVersion = module?.HasVersion(version);

			if (module is not null && hasVersion is not null && hasVersion.Value)
				return request.CreateStringResponse(HttpStatusCode.Conflict, "Module already exists with the same name and version");

			var length = request.Content.Headers.ContentLength;
			if (length is null || length.Equals(0))
				return request.CreateStringResponse(HttpStatusCode.BadRequest, $"'{HeaderNames.ContentLength}' is zero or not set");

			if(module is null)
			{
				module = new Module(@namespace, name, provider);
				_moduleContext.Add(module);
			}
			module.AddVersion(version);

			var stream = await request.Content.ReadAsStreamAsync();
			var fileKey = module.FileId(version);
			if(fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "There was an issue trying to create the new version.");
			await _storageProvider.UploadZipAsync(fileKey.Value, stream);

			_moduleContext.SaveChanges();

			return request.CreateResponse(HttpStatusCode.Created);
		}
	}
}

