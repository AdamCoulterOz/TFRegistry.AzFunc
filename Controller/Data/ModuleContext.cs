using System.Linq;
using Microsoft.EntityFrameworkCore;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data
{
	public class ModuleContext : DbContext, IItemContext
	{
		public DbSet<Module> Modules => Set<Module>();

		public ModuleContext(DbContextOptions<ModuleContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Owned<RegistryItemVersion>();
			modelBuilder.Entity<Module>().HasIndex(m => new { m.Namespace, m.Name, m.Provider }).IsUnique();
			base.OnModelCreating(modelBuilder);
		}

		public Module? GetModule(string @namespace, string name, string provider)
			=> Modules.Where(m => m.Namespace == @namespace && m.Name == name && m.Provider == provider).FirstOrDefault();

		public RegistryItem? GetItem(RegistryItem item)
		{
			var module = (Module)item;
			return GetModule(module.Namespace, module.Name, module.Provider);
		}
	}
}