using Microsoft.EntityFrameworkCore;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data;
public class ItemContext<T> : DbContext where T : class, IRegistryItem
{
	public DbSet<T> Items => Set<T>();
	protected ItemContext(DbContextOptions<ItemContext<T>> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Owned<RegistryItemVersion>();
		modelBuilder.Entity<T>().HasKey(p => p.Id);
		modelBuilder.Entity<T>().HasIndex(p => p.Id).IsUnique();
		base.OnModelCreating(modelBuilder);
	}

	public async Task<T?> GetItemAsync(T item)
		=> await GetItemAsync(item.Id);

	public async Task<T?> GetItemAsync(Address itemId)
		=> await Items.Where(item => item.Id == itemId).FirstOrDefaultAsync();
}
