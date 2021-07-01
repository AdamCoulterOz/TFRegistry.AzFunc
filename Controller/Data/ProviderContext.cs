using System.Linq;
using Microsoft.EntityFrameworkCore;
using PurpleDepot.Model;

namespace PurpleDepot.Data
{
	public class ProviderContext : DbContext
	{
		public DbSet<Provider> Providers => Set<Provider>();

		public ProviderContext(DbContextOptions<ProviderContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Provider>()
				.HasIndex(m => new { m.Namespace, m.Name })
				.IsUnique(true);
		}

		public Provider? GetProvider(string @namespace, string name)
		{
			return Providers.Where(m => m.Namespace == @namespace && m.Name == name).FirstOrDefault();
		}
	}
}