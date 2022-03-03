using Microsoft.EntityFrameworkCore;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data;

public class Repository<T> : IRepository<T> where T : RegistryItem
{
	private readonly AppContext _context;
	private readonly DbSet<T> _items;

	public Repository(AppContext context)
	{
		_context = context;
		_items = _context.Set<T>();
	}

	public async Task<T?> GetItemAsync(Address itemId)
		=> await _items.Where(item => itemId.MatchesItem<T>(item)).FirstOrDefaultAsync();

	public void Add(T item)
		=> _items.Add(item);

	public bool EnsureCreated()
		=> _context.Database.EnsureCreated();

	public void SaveChanges()
		=> _context.SaveChanges();
}
