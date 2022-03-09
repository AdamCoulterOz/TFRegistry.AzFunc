using System.Text.Json;
using PurpleDepot.Core.Interface.Model;
using PurpleDepot.Core.Interface.Model.Module;
using PurpleDepot.Core.Interface.Model.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PurpleDepot.Core.Controller.Data;
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
			pe.OwnsMany(p => p.Versions, pve =>
			{
				pve.OwnsMany(pv => pv.Platforms);
				pve.Property(pv => pv.Protocols)
					.HasConversion(ListConverter<string>(), ListComparer<string>());
			});
		});
		modelBuilder.Entity<Module>(me =>
		{
			me.OwnsMany(m => m.Versions);
			me.Property(m => m.Providers)
				.HasConversion(ListConverter<string>(), ListComparer<string>());
		});
	}

	private ValueComparer ListComparer<T>() where T : notnull
		=> new ValueComparer<List<T>>(
			(first, second)
				=> first != null && second != null
					? first.SequenceEqual(second)
					: first == null && second == null
						? true : false,
			c => c.Aggregate(0, (accumulate, value) => HashCode.Combine(accumulate, value.GetHashCode())),
			c => c.ToList());

	private static ValueConverter<List<T>, string> ListConverter<T>()
		=> new(
			v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
			v => JsonSerializer.Deserialize<List<T>>(v, new JsonSerializerOptions()) ?? new List<T>());

	private static void Builder<T>(ModelBuilder modelBuilder)
		where T : RegistryItem<T>
	{
		modelBuilder.Entity<T>(rie =>
		{
			rie.HasBaseType<RegistryItem<T>>();
			rie.Ignore(ri => ri.Address);
			rie.Ignore(ri => ri.Version);

			modelBuilder.Entity<RegistryItem<T>>(ribe =>
			{
				ribe.HasKey(ri => ri.Id);
				ribe.HasIndex(ri => ri.Id)
					.IsUnique();
			});
		});
	}
}
