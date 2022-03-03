using System.Net;
using System.Web;
using PurpleDepot.Controller.Exceptions;
using PurpleDepot.Data;
using PurpleDepot.Interface.Model;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Controller;
public class ItemController<T>
	where T : RegistryItem<T>
{
	protected readonly IRepository<T> _itemRepo;
	protected readonly IStorageProvider<T> _storageProvider;

	public ItemController(IRepository<T> itemRepo, IStorageProvider<T> storageProvider)
	{
		_itemRepo = itemRepo;
		_itemRepo.EnsureCreated();
		_storageProvider = storageProvider;
	}

	public async Task<HttpResponseMessage> IngestAsync(HttpRequestMessage request, Address<T> newAddress, string version, Stream stream)
	{
		var item = await _itemRepo.GetItemAsync(newAddress);

		if (item is not null && item.HasVersion(version))
			throw new AlreadyExistsException<T>(item.Address, request);

		if (item is null)
		{
			item = newAddress.NewItem(version);
			_itemRepo.Add(item);
		}
		else
			item.AddVersion(version);

		var newVersion = item.GetVersion(version)!;
		if (await _storageProvider.UploadZipAsync(item.GetFileKey(newVersion), stream))
		{
			_itemRepo.SaveChanges();
			return request.CreateResponse(HttpStatusCode.Created);
		}
		else
			return request.CreateStringResponse(HttpStatusCode.BadRequest, "Uploading file failed, and the module was not saved.");
	}

	public async Task<HttpResponseMessage> DownloadAsync(HttpRequestMessage request, Address<T> itemId, string? versionName = null)
	{
		(var item, var version) = await GetItemAsync(request, itemId, versionName);
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

	public async virtual Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, Address<T> itemId)
		=> await GetAsync(request, itemId, versionName: null);

	public async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, Address<T> itemId, string? versionName = null)
		=> request.CreateJsonResponse(await GetItemAsync(request, itemId, versionName));

	protected async Task<(T item, RegistryItemVersion version)> GetItemAsync(HttpRequestMessage request, Address<T> itemId, string? versionName = null)
	{
		var item = await _itemRepo.GetItemAsync(itemId);
		var version = item?.GetVersion(versionName);
		if (item is null || version is null)
			throw (new NotFoundException<T>(itemId, request));
		return (item, version);
	}
}
