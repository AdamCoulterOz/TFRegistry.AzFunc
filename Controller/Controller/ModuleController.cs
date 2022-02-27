using PurpleDepot.Data;
using PurpleDepot.Interface.Model;
using PurpleDepot.Interface.Model.Module;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Controller;
public class ModuleController : ItemController<Module>
{
	public ModuleController(ItemContext<Module> itemContext, IStorageProvider<Module> storageProvider) : base(itemContext, storageProvider) { }

	public override async Task<HttpResponseMessage> VersionsAsync(HttpRequestMessage request, Address itemId)
		=> request.CreateJsonResponse(new ModuleCollection(new List<Module> { (await LatestAsync(itemId)) }));
}
