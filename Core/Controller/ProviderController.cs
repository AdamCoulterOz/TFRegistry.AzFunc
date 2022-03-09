using PurpleDepot.Core.Controller.Data;
using PurpleDepot.Core.Interface.Model.Provider;
using PurpleDepot.Core.Interface.Storage;

namespace PurpleDepot.Core.Controller;
public class ProviderController : ItemController<Provider>
{
	protected ProviderController(IRepository<Provider> itemRepo, IStorageProvider<Provider> storageProvider) : base(itemRepo, storageProvider) { }
}
