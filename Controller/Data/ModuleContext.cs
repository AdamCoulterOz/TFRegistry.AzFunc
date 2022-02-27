using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Data
{
	public class ModuleContext : ItemContext
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

		public override RegistryItem? GetItem(RegistryItem item)
		{
			var module = (Module)item;
			return GetModule(module.Namespace, module.Name, module.Provider);
		}
	}
}