using System.Net;
using System.Web;
using Controller.Data;
using Controller.Exceptions;
using Interface.Model;
using Interface.Storage;

namespace Controller;

public class ItemController<T>
	where T : RegistryItem<T>
{
	private readonly IRepository<T> _itemRepo;
	private readonly IStorageProvider<T> _storageProvider;

	protected ItemController(IRepository<T> itemRepo, IStorageProvider<T> storageProvider)
	{
		_itemRepo = itemRepo;
		_itemRepo.EnsureCreated();
		_storageProvider = storageProvider;
	}

	protected async Task<HttpResponseMessage> IngestAsync(HttpRequestMessage request, Address<T> newAddress,
		string version, Stream stream)
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
		try
		{
			await _storageProvider.UploadZipAsync(item.GetFileKey(newVersion), stream);
		}
		catch(Exception e)
		{
			throw new HttpResponseException(request, HttpStatusCode.InternalServerError, $"Uploading file failed, and the module was not saved. Inner error: {e.Message}");
		}

		_itemRepo.SaveChanges();
		return request.CreateResponse(HttpStatusCode.Created);
	}

	protected async Task<HttpResponseMessage> DownloadAsync(HttpRequestMessage request, Address<T> itemId,
		string? versionName = null)
	{
		var (item, version) = await GetItemAsync(request, itemId, versionName);
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

	protected virtual async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, Address<T> itemId, string? versionName = null)
		=> request.CreateJsonResponse(await GetItemAsync(request, itemId, versionName));

	protected async Task<(T item, RegistryItemVersion version)> GetItemAsync(HttpRequestMessage request,
		Address<T> itemId, string? versionName = null)
	{
		var item = await _itemRepo.GetItemAsync(itemId);
		var version = item?.GetVersion(versionName);
		if (item is null || version is null)
			throw new NotFoundException<T>(itemId, request);
		return (item, version);
	}
}
