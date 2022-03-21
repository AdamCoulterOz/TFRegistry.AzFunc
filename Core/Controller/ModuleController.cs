using PurpleDepot.Core.Controller.Data;
using PurpleDepot.Core.Interface.Model;
using PurpleDepot.Core.Interface.Model.Module;
using PurpleDepot.Core.Interface.Storage;

namespace PurpleDepot.Core.Controller;

public class ModuleController : ItemController<Module>
{
	protected ModuleController(IRepository<Module> itemRepo, IStorageProvider<Module> storageProvider)
		: base(itemRepo, storageProvider) { }

	protected override async Task<ControllerResult> GetAsync(Address<Module> itemId,
		string? versionName = null)
	{
		var modules = new List<Module>();
		var item = await GetItemAsync(itemId, versionName);

		modules.Add(item.item);

		var response = new ModuleCollection(modules);
		return ControllerResult.NewJson(response);
	}
}
