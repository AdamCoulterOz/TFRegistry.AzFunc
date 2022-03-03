using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data;

public interface IRepository<T> where T : RegistryItem
{
	void Add(T item);
	bool EnsureCreated();
	Task<T?> GetItemAsync(Address itemId);
	void SaveChanges();
}
