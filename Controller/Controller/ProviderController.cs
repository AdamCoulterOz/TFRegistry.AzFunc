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
	public class ProviderController
	{
		private readonly ProviderContext _providerContext;
		private readonly IStorageProvider _storageProvider;
		public ProviderController(ProviderContext ProviderContext, IStorageProvider storageProvider)
		{
			_providerContext = ProviderContext;
			_storageProvider = storageProvider;
			_providerContext.Database.EnsureCreated();
		}

		public async Task<HttpResponseMessage> DownloadProviderAsync(HttpRequestMessage request, Guid fileKey, string fileName)
		{
			var (blobDownloadStream, blobContentLength) = await _storageProvider.DownloadZipAsync(fileKey);
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

			var providerCollection = new ProviderCollection();
			providerCollection.Providers.Add(provider);
			return request.CreateJsonResponse(providerCollection);
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

		public async Task<HttpResponseMessage> Ingest(
			HttpRequestMessage request,
			string @namespace, string name, string version)
		{
			request.Authenticate();

			if (request.Content is null)
				return request.CreateResponse(HttpStatusCode.BadRequest);

			var provider = _providerContext.GetProvider(@namespace, name);
			var hasVersion = provider?.HasVersion(version);

			if (provider is not null && hasVersion is not null && hasVersion.Value)
				return request.CreateStringResponse(HttpStatusCode.Conflict, "Provider already exists with the same name and version");

			var length = request.Content.Headers.ContentLength;
			if (length is null || length.Equals(0L))
				return request.CreateStringResponse(HttpStatusCode.BadRequest, $"'{HeaderNames.ContentLength}' is zero or not set");

			if (provider is null)
			{
				provider = new Provider(@namespace, name);
				_providerContext.Add(provider);
			}
			provider.AddVersion(version);

			var stream = await request.Content.ReadAsStreamAsync();
			var fileKey = provider.FileId(version);
			if (fileKey is null)
				return request.CreateStringResponse(HttpStatusCode.InternalServerError, "There was an issue trying to create the new version.");

			bool hadContent = await _storageProvider.UploadZipAsync(fileKey.Value, stream);

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

