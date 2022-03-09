using PurpleDepot.Core.Controller.Data;
using PurpleDepot.Core.Interface.Model;
using PurpleDepot.Core.Interface.Model.Module;
using PurpleDepot.Core.Interface.Storage;

namespace PurpleDepot.Core.Controller;

public class ModuleController : ItemController<Module>
{
	protected ModuleController(IRepository<Module> itemRepo, IStorageProvider<Module> storageProvider)
		: base(itemRepo, storageProvider) { }

	protected override async Task<HttpResponseMessage> GetAsync(HttpRequestMessage request, Address<Module> itemId,
		string? versionName = null)
		=> request.CreateJsonResponse(new ModuleCollection(new List<Module>
			{(await GetItemAsync(request, itemId, versionName)).item}));
}
