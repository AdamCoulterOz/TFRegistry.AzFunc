using PurpleDepot.Data;
using PurpleDepot.Interface.Model.Provider;
using PurpleDepot.Interface.Storage;

namespace PurpleDepot.Controller;
public class ProviderController : ItemController<Provider>
{
	public ProviderController(IRepository<Provider> itemRepo, IStorageProvider<Provider> storageProvider) : base(itemRepo, storageProvider) { }
}
