using System.Linq;
using System.Threading.Tasks;
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
				.HasMany(module => module.Versions);

			modelBuilder.Entity<Module>()
				.HasKey(module => module.PrimaryKey);
		}

		public async Task<Module> GetModule(string @namespace, string name, string provider, string version = "latest")
		{
			return version.Equals("latest") ?
				await Modules.FindAsync(new string[] { @namespace, name, provider })
			 :  await Modules.Where(m => m.Namespace == @namespace)
							 .Where(m => m.Name == name)
							 .Where(m => m.Provider == provider)
						  	 .Where(m => m.Versions.Any(ve => ve.Version == version))
						  	 .SingleAsync();
		}
	}
}