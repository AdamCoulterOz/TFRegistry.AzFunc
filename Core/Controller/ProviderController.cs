using Controller.Data;
using Interface.Model.Provider;
using Interface.Storage;

namespace Controller.Controller;
public class ProviderController : ItemController<Provider>
{
	protected ProviderController(IRepository<Provider> itemRepo, IStorageProvider<Provider> storageProvider) : base(itemRepo, storageProvider) { }
}
