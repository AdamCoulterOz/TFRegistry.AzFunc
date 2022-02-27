using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PurpleDepot.Data;
using PurpleDepot.Interface.Storage;
using System;
using System.Web;
using PurpleDepot.Interface.Exceptions;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Controller
{
	public class ModuleController : ItemController
	{
		public ModuleController(ModuleContext moduleContext, IStorageProvider storageProvider)
			: base(moduleContext, storageProvider)
				=> ((ModuleContext)_itemContext).Database.EnsureCreated();

		protected override bool Validate(RegistryItem item)
		{
			var module = (Module)item;
			return module.Version is not null;
		}

		protected override bool HasVersion(RegistryItem? item, RegistryItemVersion version)
		{
			var hasVersion = item?.HasVersion(version.Version);
			return item is not null && hasVersion is not null && hasVersion.Value;
		}

		public async Task<HttpResponseMessage> DownloadModuleAsync(HttpRequestMessage request, Guid fileKey, string fileName)
		{
			var (blobDownloadStream, blobContentLength) = await _storageProvider.DownloadZipAsync(fileKey, ObjectType.Module);
			if (blobDownloadStream is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Cannot find module zip.");
			return request.CreateZipDownloadResponse(blobDownloadStream, fileName, blobContentLength);
		}

		public HttpResponseMessage Versions(
			HttpRequestMessage request,
			string @namespace, string name, string provider)
		{
			var module = _itemContext.GetModule(@namespace, name, provider);
			if (module is null)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var moduleCollection = new ModuleCollection();
			moduleCollection.Modules.Add(module);
			return request.CreateJsonResponse(moduleCollection);
		}

		public HttpResponseMessage Download(
			HttpRequestMessage request, string @namespace, string name, string provider)
			=> DownloadSpecific(request, @namespace, name, provider, "latest");

		public HttpResponseMessage DownloadSpecific(
			HttpRequestMessage request, string @namespace, string name, string provider, string version)
		{
			var module = _itemContext.GetItem(@namespace, name, provider);
			if (module is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module doesn't exist at any version.");
			if (!module.HasVersion(version))
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Module found, but specified version doesn't exist.");
			var fileKey = module.FileId(version);
			if (fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "Couldn't get file key.");
			var response = request.CreateResponse(HttpStatusCode.NoContent);
			var downloadUri = _storageProvider.DownloadLink(fileKey.Value, ObjectType.Module);
			var builder = new UriBuilder(downloadUri);
			var query = HttpUtility.ParseQueryString(builder.Query);
			query.Add("archive", "zip");
			builder.Query = query.ToString();
			response.Headers.Add("X-Terraform-Get", builder.ToString());
			return response;
		}

		public HttpResponseMessage Latest(Module moduleRequest)
			=> Specific(moduleRequest, "latest");

		public DataModule Specific(HttpRequestMessage request, string namespace, string name, string version)
		{
			var module = _itemContext.GetItem(request.Namespace, request.Name, request.Provider);
			if (module is null)
				throw new NotFoundException(request);
			if (!module.HasVersion(version))
				throw new NotFoundException(request);
			return request.CreateJsonResponse(module);
		}
	}
}

