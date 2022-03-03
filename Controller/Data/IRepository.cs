using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data;

public interface IRepository<T>
	where T : RegistryItem<T>
{
	void Add(T item);
	bool EnsureCreated();
	Task<T?> GetItemAsync(Address<T> itemId);
	void SaveChanges();
}
