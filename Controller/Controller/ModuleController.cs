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
		}

		public async Task<HttpResponseMessage> Versions(
			HttpRequestMessage request,
			string @namespace, string name, string provider)
		{
			request.Authenticate();

			var module = await _moduleContext.GetModule(@namespace, name, provider);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var moduleCollection = new ModuleCollection();
			moduleCollection.Modules.Add(module);
			return request.CreateJsonResponse(moduleCollection);
		}

		public async Task<HttpResponseMessage> Download(
			HttpRequestMessage request, string @namespace, string name, string provider)
		{
			request.Authenticate();
			return await DownloadSpecific(request, @namespace, name, provider, "latest");
		}

		public async Task<HttpResponseMessage> DownloadSpecific(
			HttpRequestMessage request, string @namespace, string name, string provider, string version)
		{
			request.Authenticate();

			var module = await _moduleContext.GetModule(@namespace, name, provider, version);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);
			var downloadStream = _storageProvider.DownloadZip(module.FileKey);
			if (downloadStream is null)
				return request.CreateResponse(HttpStatusCode.NotFound);
			return request.CreateZipDownloadResponse(downloadStream, module.FileName(version));
		}

		public async Task<HttpResponseMessage> Latest(
			HttpRequestMessage request,
			string @namespace, string name, string provider)
		{
			request.Authenticate();
			return await Specific(request, @namespace, name, provider, "latest");
		}

		public async Task<HttpResponseMessage> Specific(
			HttpRequestMessage request,
			string @namespace, string name, string provider, string version)
		{
			request.Authenticate();
			var module = await _moduleContext.GetModule(@namespace, name, provider, version);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);
			return request.CreateJsonResponse(module);
		}

		public async Task<HttpResponseMessage> Ingest(
			HttpRequestMessage request,
			string @namespace, string name, string provider, string version)
		{
			request.Authenticate();

			if (request.Content is null)
				return request.CreateResponse(HttpStatusCode.BadRequest);

			var module = await _moduleContext.GetModule(@namespace, name, provider, version);
			if (module is not null)
				return request.CreateStringResponse(HttpStatusCode.Conflict, "Module already exists with the same name and version");

			var length = request.Content.Headers.ContentLength;
			if (length is null || length.Equals(0))
				return request.CreateStringResponse(HttpStatusCode.BadRequest, $"'{HeaderNames.ContentLength}' is zero or not set");

			module = new Module(@namespace, name, provider, version);
			module.Versions.Add(new VersionElement(){Version = version, Module = module});

			using var stream = await request.Content.ReadAsStreamAsync();
			await _storageProvider.UploadZip(module.FileKey, stream);

			_moduleContext.Add(module);
			_moduleContext.SaveChanges();

			return request.CreateResponse(HttpStatusCode.Created);
		}
	}
}

