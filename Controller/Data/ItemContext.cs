using Microsoft.EntityFrameworkCore;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data;

public abstract class ItemContext : DbContext
{
	public ItemContext(DbContextOptions<ItemContext> options) : base(options) { }
	public abstract RegistryItem? GetItem(RegistryItem item);
}
