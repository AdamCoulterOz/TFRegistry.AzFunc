using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PurpleDepot.Data;
using PurpleDepot.Model;
using PurpleDepot.Interface.Storage;
using System;
using System.Web;
using PurpleDepot.Interface.Host;
using PurpleDepot.Interface.Exceptions;
using System.IO;
using System.Collections.Generic;
using PurpleDepot.Interface.Model;
using System.Linq;

namespace PurpleDepot.Controller
{
	public class ProviderController : ItemController
	{
		public ProviderController(ProviderContext providerContext, IStorageProvider storageProvider)
			: base(providerContext, storageProvider)
				=> ((ProviderContext)_itemContext).Database.EnsureCreated();

		protected override bool Validate(RegistryItem item)
		{
			var provider = (Provider)item;
			var providerVersion = (ProviderVersion)provider.Version;
			return providerVersion is not null
					&& providerVersion.Platforms.Count > 0
					&& providerVersion.Protocols.Count > 0;
		}

		protected override bool HasVersion(RegistryItem item, RegistryItemVersion version)
		{
			var hasVersion = item.HasVersion(version.Version);
			var existingVersion = (ProviderVersion)item.Version;
			var newVersion = (ProviderVersion)version;

			return hasVersion
				&& existingVersion.Platforms.Contains(newVersion.Platforms.First());
		}

		public async Task<HttpResponseMessage> DownloadProviderAsync(HttpRequestMessage request, Guid fileKey, string fileName)
		{
			var (blobDownloadStream, blobContentLength) = await _storageProvider.DownloadZipAsync(fileKey, ObjectType.Provider);
			if (blobDownloadStream is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Cannot find provider zip.");
			return request.CreateZipDownloadResponse(blobDownloadStream, fileName, blobContentLength);
		}

		public HttpResponseMessage Versions(
			HttpRequestMessage request,
			string @namespace, string name)
		{
			request.Authenticate();

			var provider = _providerContext.GetProvider(@namespace, name);
			if (provider is null)
				return request.CreateResponse(HttpStatusCode.NotFound);

			var providerVersionCollection = new ProviderVersionCollection
			{
				Versions = provider.Versions
			};
			return request.CreateJsonResponse(providerVersionCollection);
		}

		public HttpResponseMessage Download(
			HttpRequestMessage request, string @namespace, string name)
		{
			return DownloadSpecific(request, @namespace, name, "latest");
		}

		public HttpResponseMessage DownloadSpecific(
			HttpRequestMessage request, string @namespace, string name, string version)
		{
			request.Authenticate();
			var provider = _providerContext.GetProvider(@namespace, name);
			if (provider is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Provider doesn't exist at any version.");
			if (!provider.HasVersion(version))
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Provider found, but specified version doesn't exist.");
			var fileKey = provider.FileId(version);
			if (fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "Couldn't get file key.");
			var response = request.CreateResponse(HttpStatusCode.NoContent);
			var downloadUri = _storageProvider.DownloadLink(fileKey.Value, ObjectType.Provider);
			var builder = new UriBuilder(downloadUri);
			var query = HttpUtility.ParseQueryString(builder.Query);
			query.Add("archive", "zip");
			builder.Query = query.ToString();
			response.Headers.Add("X-Terraform-Get", builder.ToString());
			return response;
		}

		public HttpResponseMessage Latest(
			HttpRequestMessage request,
			string @namespace, string name)
		{
			request.Authenticate();
			return Specific(request, @namespace, name, "latest");
		}

		public HttpResponseMessage Specific(
			HttpRequestMessage request,
			string @namespace, string name, string version)
		{
			request.Authenticate();
			var provider = _providerContext.GetProvider(@namespace, name);
			if (provider is null)
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Provider doesn't exist at any version.");
			if (!provider.HasVersion(version))
				return request.CreateStringResponse(HttpStatusCode.NotFound, "Provider doesn't exist at specified version.");
			return request.CreateJsonResponse(provider);
		}

		public async Task Ingest(Provider request, Stream content)
		{


			provider.AddVersion(request.Version, request.Protocols, platforms);

			var fileKey = provider.FileId(request.Version);
			if (fileKey is null)
				throw new Exception("There was an issue trying to create the new version.");

			bool hadContent = await _storageProvider.UploadZipAsync(fileKey.Value, content, ObjectType.Provider);

			if (hadContent)
			{
				_providerContext.SaveChanges();
				return request.CreateResponse(HttpStatusCode.Created);
			}
			else
			{
				return request.CreateStringResponse(HttpStatusCode.BadRequest, "Uploaded file had zero bytes, and the provider was not saved.");
			}
		}
	}
}

