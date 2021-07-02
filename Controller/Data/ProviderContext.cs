using System.Linq;
using Microsoft.EntityFrameworkCore;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data
{
	public class ProviderContext : DbContext, IItemContext
	{
		public DbSet<Provider> Providers => base.Set<Provider>();

		public ProviderContext(DbContextOptions<ProviderContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Owned<ProviderPlatform>();
			modelBuilder.Owned<ProviderVersion>();
			modelBuilder.Entity<Provider>().HasKey(p => p.Id);
			modelBuilder.Entity<Provider>().HasIndex(p => (new { p.Namespace, p.Name })).IsUnique();
			base.OnModelCreating(modelBuilder);
		}

		public Provider? GetProvider(string @namespace, string name)
			=> Providers.Where(p => p.Namespace == @namespace && p.Name == name).FirstOrDefault();

		public RegistryItem? GetItem(RegistryItem item)
		{
			var provider = (Provider)item;
			return GetProvider(provider.Namespace, provider.Name);
		}
	}
}