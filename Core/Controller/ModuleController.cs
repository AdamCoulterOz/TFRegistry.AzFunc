using Controller.Data;
using Interface.Model;
using Interface.Model.Module;
using Interface.Storage;

namespace Controller.Controller;

public class ModuleController : ItemController<Module>
{
	protected ModuleController(IRepository<Module> itemRepo, IStorageProvider<Module> storageProvider)
		: base(itemRepo, storageProvider) { }

	protected override async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, Address<Module> itemId,
		string? versionName = null)
		=> request.CreateJsonResponse(new ModuleCollection(new List<Module>
			{(await GetItemAsync(request, itemId, versionName)).item}));
}
