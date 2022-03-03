using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PurpleDepot.Interface.Model;
using PurpleDepot.Interface.Model.Module;
using PurpleDepot.Interface.Model.Provider;
using System.Text.Json;

namespace PurpleDepot.Data;
public class AppContext : DbContext
{
	public AppContext(DbContextOptions<AppContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		Builder<Module>(modelBuilder);
		Builder<Provider>(modelBuilder);

		modelBuilder.Entity<Provider>(pe =>
		{
			pe.OwnsMany<ProviderVersion>(p => p.Versions, pve =>
			{
				pve.OwnsMany<ProviderPlatform>(pv => pv.Platforms);
				pve.Property(pv => pv.Protocols)
					.HasConversion(ListConverter<string>());
			});
		});
		modelBuilder.Entity<Module>(me =>
		{
			me.OwnsMany<ModuleVersion>(m => m.Versions);
			me.Property(m => m.Providers)
				.HasConversion(ListConverter<string>());
		});
	}

	public static ValueConverter<List<T>, string> ListConverter<T>()
		=> new ValueConverter<List<T>, string>(
				 v => JsonSerializer.Serialize<List<T>>(v, new JsonSerializerOptions()),
				 v => JsonSerializer.Deserialize<List<T>>(v, new JsonSerializerOptions()) ?? new List<T>());

	private void Builder<RI>(ModelBuilder modelBuilder)
		where RI : RegistryItem
	{
		modelBuilder.Entity<RI>(rie =>
		{
			rie.HasBaseType<RegistryItem>();
			rie.Ignore(ri => ri.Address);
			rie.Ignore(ri => ri.Version);

			modelBuilder.Entity<RegistryItem>(ribe =>
			{
				ribe.HasKey(ri => ri.Id);
				ribe.HasIndex(ri => ri.Id)
					.IsUnique();
			});
		});
	}
}
