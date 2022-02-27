using System.Net;
using System.Web;
using PurpleDepot.Data;
using PurpleDepot.Interface.Exceptions;
using PurpleDepot.Interface.Model;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Controller;
public class ItemController<T> where T : class, IRegistryItem
{
	protected readonly ItemContext<T> _itemContext;
	protected readonly IStorageProvider<T> _storageProvider;

	public ItemController(ItemContext<T> itemContext, IStorageProvider<T> storageProvider)
	{
		_itemContext = itemContext;
		_itemContext.Database.EnsureCreated();
		_storageProvider = storageProvider;
	}

	public async Task<HttpResponseMessage> IngestAsync(HttpRequestMessage request, T newItem, Stream stream)
	{
		var item = await _itemContext.GetItemAsync(newItem.Id);

		if (item is not null && item.HasVersion(newItem.Version.Version))
			throw new AlreadyExistsException(item);

		if (item is null)
		{
			item = newItem;
			_itemContext.Add(item);
		}
		else
			item.AddVersion(newItem.Version);

		if (await _storageProvider.UploadZipAsync(item.GetFileKey(newItem.Version), stream))
		{
			_itemContext.SaveChanges();
			return request.CreateResponse(HttpStatusCode.Created);
		}
		else
			return request.CreateStringResponse(HttpStatusCode.BadRequest, "Uploading file failed, and the module was not saved.");
	}

	public async virtual Task<HttpResponseMessage> VersionsAsync(HttpRequestMessage request, Address itemId)
		=> request.CreateJsonResponse(await LatestAsync(itemId));

	public async Task<HttpResponseMessage> DownloadAsync(HttpRequestMessage request, Address itemId)
		=> await DownloadSpecificAsync(request, itemId);

	public async Task<HttpResponseMessage> DownloadSpecificAsync(HttpRequestMessage request, Address itemId, string? versionName = null)
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

	public async Task<T> LatestAsync(Address itemId)
		=> (await SpecificAsync(itemId)).item;

	public async Task<(T item, RegistryItemVersion version)> SpecificAsync(Address itemId, string? versionName = null)
	{
		var item = await _itemContext.GetItemAsync(itemId);
		if (item is null)
			throw new NotFoundException(itemId);
		var version = item.GetVersion(versionName);
		if (version is null)
			throw new NotFoundException(itemId);
		return (item, version);
	}
}
