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
				.HasMany(module => module.Versions)
				.WithOne(version => version.Module);

			modelBuilder.Entity<Module>()
				.HasKey(module => new { module.Namespace, module.Name, module.Provider });

		}

		public async Task<Module?> GetModule(string @namespace, string name, string provider, string version = "latest")
		{
			return version.Equals("latest") ?
				await Modules.FindAsync(new { @namespace, name, provider })
			 : Modules.Where(m => m.Namespace == @namespace && m.Name == name && m.Provider == provider).FirstOrDefault();
			 				// .Include(module => module.Versions)
						  	// .Where(m => m.Versions.Any(ve => ve.Version == version))
							// .FirstOrDefaultAsync();
		}
	}
}