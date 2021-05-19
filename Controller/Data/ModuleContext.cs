using System.Linq;
using Microsoft.EntityFrameworkCore;
using PurpleDepot.Model;

namespace PurpleDepot.Data
{
	public class ModuleContext : DbContext
	{
		public DbSet<Module> Modules => Set<Module>();

		public ModuleContext(DbContextOptions<ModuleContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Module>()
				.HasIndex(m => new { m.Namespace, m.Name, m.Provider })
				.IsUnique(true);
		}

		public Module? GetModule(string @namespace, string name, string provider)
		{
			return Modules.Where(m => m.Namespace == @namespace && m.Name == name && m.Provider == provider).FirstOrDefault();
		}
	}
}