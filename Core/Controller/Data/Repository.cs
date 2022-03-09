using PurpleDepot.Core.Interface.Model;
using Microsoft.EntityFrameworkCore;

namespace PurpleDepot.Core.Controller.Data;

public class Repository<T> : IRepository<T>
	where T : RegistryItem<T>
{
	private readonly AppContext _context;
	private readonly DbSet<T> _items;

	public Repository(AppContext context)
	{
		_context = context;
		_items = _context.Set<T>();
	}

	public async Task<T?> GetItemAsync(Address<T> itemId)
		=> await _items.Where(item => item.Id == itemId.ToString()).FirstOrDefaultAsync();

	public void Add(T item)
		=> _items.Add(item);

	public void EnsureCreated()
		=> _context.Database.EnsureCreated();

	public void SaveChanges()
		=> _context.SaveChanges();
}
