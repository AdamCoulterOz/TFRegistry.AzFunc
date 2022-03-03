using PurpleDepot.Data;
using PurpleDepot.Interface.Model;
using PurpleDepot.Interface.Model.Module;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Controller;
public class ModuleController : ItemController<Module>
{
	public ModuleController(IRepository<Module> itemRepo, IStorageProvider<Module> storageProvider) : base(itemRepo, storageProvider) { }

	public override async Task<HttpResponseMessage> VersionsAsync(HttpRequestMessage request, Address<Module> itemId)
		=> request.CreateJsonResponse(new ModuleCollection(new List<Module> { (await LatestAsync(itemId)) }));
}
