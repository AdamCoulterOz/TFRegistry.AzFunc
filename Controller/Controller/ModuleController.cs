using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PurpleDepot.Data;
using PurpleDepot.Model;
using PurpleDepot.Interface.Storage;
using Microsoft.Net.Http.Headers;
using System;
using System.Web;

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

		public async Task<HttpResponseMessage> DownloadModuleAsync(HttpRequestMessage request, Guid fileKey, string fileName)
		{
			var (blobDownloadStream, blobContentLength) = await _storageProvider.DownloadZipAsync(fileKey);
			if (blobDownloadStream is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Cannot find module zip.");
			return request.CreateZipDownloadResponse(blobDownloadStream, fileName, blobContentLength);
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

		public HttpResponseMessage Download(
			HttpRequestMessage request, string @namespace, string name, string provider)
		{
			return DownloadSpecific(request, @namespace, name, provider, "latest");
		}

		public HttpResponseMessage DownloadSpecific(
			HttpRequestMessage request, string @namespace, string name, string provider, string version)
		{
			request.Authenticate();
			var module = _moduleContext.GetModule(@namespace, name, provider);
			if (module is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module doesn't exist at any version.");
			if (!module.HasVersion(version))
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module found, but specified version doesn't exist.");
			var fileKey = module.FileId(version);
			if (fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "Couldn't get file key.");
			var response = request.CreateResponse(HttpStatusCode.NoContent);
			var downloadUri = _storageProvider.DownloadLink(fileKey.Value);
			var builder = new UriBuilder(downloadUri);
			var query = HttpUtility.ParseQueryString(builder.Query);
			query.Add("archive", "zip");
			builder.Query = query.ToString();
			response.Headers.Add("X-Terraform-Get", builder.ToString());
			return response;
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
			if (length is null || length.Equals(0L))
				return request.CreateStringResponse(HttpStatusCode.BadRequest, $"'{HeaderNames.ContentLength}' is zero or not set");

			if (module is null)
			{
				module = new Module(@namespace, name, provider);
				_moduleContext.Add(module);
			}
			module.AddVersion(version);

			var stream = await request.Content.ReadAsStreamAsync();
			var fileKey = module.FileId(version);
			if (fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "There was an issue trying to create the new version.");

			bool hadContent = await _storageProvider.UploadZipAsync(fileKey.Value, stream);

			if (hadContent)
			{
				_moduleContext.SaveChanges();
				return request.CreateResponse(HttpStatusCode.Created);
			}
			else
			{
				return request.CreateStringResponse(HttpStatusCode.BadRequest, "Uploaded file had zero bytes, and the module was not saved.");
			}
		}
	}
}

