using Interface.Model;

namespace Controller.Data;

public interface IRepository<T>
	where T : RegistryItem<T>
{
	void Add(T item);
	void EnsureCreated();
	Task<T?> GetItemAsync(Address<T> itemId);
	void SaveChanges();
}
