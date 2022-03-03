using System.Net;
using System.Web;
using PurpleDepot.Data;
using PurpleDepot.Interface.Exceptions;
using PurpleDepot.Interface.Model;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Controller;
public class ItemController<RI>
	where RI : RegistryItem
{
	protected readonly IRepository<RI> _itemRepo;
	protected readonly IStorageProvider<RI> _storageProvider;

	public ItemController(IRepository<RI> itemRepo, IStorageProvider<RI> storageProvider)
	{
		_itemRepo = itemRepo;
		_itemRepo.EnsureCreated();
		_storageProvider = storageProvider;
	}

	public async Task<HttpResponseMessage> IngestAsync(HttpRequestMessage request, RI newItem, Stream stream)
	{
		var item = await _itemRepo.GetItemAsync(newItem.Address);

		if (item is not null && item.HasVersion(newItem.Version.Version))
			throw new AlreadyExistsException(item);

		if (item is null)
		{
			item = newItem;
			_itemRepo.Add(item);
		}
		else
			item.AddVersion(newItem.Version);

		if (await _storageProvider.UploadZipAsync(item.GetFileKey(newItem.Version), stream))
		{
			_itemRepo.SaveChanges();
			return request.CreateResponse(HttpStatusCode.Created);
		}
		else
			return request.CreateStringResponse(HttpStatusCode.BadRequest, "Uploading file failed, and the module was not saved.");
	}

	public async virtual Task<HttpResponseMessage> VersionsAsync(HttpRequestMessage request, Address itemId)
		=> request.CreateJsonResponse(await LatestAsync(itemId));

	public async Task<HttpResponseMessage> DownloadAsync(HttpRequestMessage request, Address itemId)
		=> await DownloadVersionAsync(request, itemId);

	public async Task<HttpResponseMessage> DownloadVersionAsync(HttpRequestMessage request, Address itemId, string? versionName = null)
	{
		(var item, var version) = await SpecificAsync(itemId, versionName);
		var fileKey = item.GetFileKey(version);

		var response = request.CreateResponse(HttpStatusCode.NoContent);
		var downloadUri = _storageProvider.DownloadLink(fileKey);
		var builder = new UriBuilder(downloadUri);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query.Add("archive", "zip");
		builder.Query = query.ToString();
		response.Headers.Add("X-Terraform-Get", builder.ToString());
		return response;
	}

	protected async Task<RI> LatestAsync(Address itemId)
		=> (await SpecificAsync(itemId)).item;

	public async Task<HttpResponseMessage> LatestAsync(HttpRequestMessage request, Address itemId)
		=> request.CreateJsonResponse(await LatestAsync(itemId));

	private async Task<(RI item, RegistryItemVersion version)> SpecificAsync(Address itemId, string? versionName = null)
	{
		var item = await _itemRepo.GetItemAsync(itemId);
		if (item is null)
			throw new NotFoundException(itemId);
		var version = item.GetVersion(versionName);
		if (version is null)
			throw new NotFoundException(itemId);
		return (item, version);
	}

	public async Task<HttpResponseMessage> VersionAsync(HttpRequestMessage request, Address itemId, string? versionName = null)
		=> request.CreateJsonResponse(await SpecificAsync(itemId, versionName));
}
