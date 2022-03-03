using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PurpleDepot.Data;
using PurpleDepot.Interface.Storage;
using PurpleDepot.Providers.Azure.Storage;
using PurpleDepot.Providers.Azure.Database;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Providers.Azure;
public class Program
{
	const string AppName = nameof(PurpleDepot);
	public static void Main()
	{
		var host = new HostBuilder()
			.ConfigureFunctionsWorkerDefaults()
			.ConfigureServices(ConfigureServices)
			.Build();

		host.Run();
	}

	static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		context.Configuration = new ConfigurationBuilder()
								.AddEnvironmentVariables(AppName)
								.Build();

		string service = context.Configuration.GetValue<string>("Provider");
		switch (service)
		{
			case "Azure": AzureServices(context, services); break;
			default: DevServices(context, services); break;
		}
		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
	}

	static void DevServices(HostBuilderContext _, IServiceCollection services)
	{
		services.AddScoped(typeof(IStorageProvider<>), typeof(MockStorageService<>));
		services.AddDbContext<Data.AppContext>(options => options.UseInMemoryDatabase(AppName));
	}

	static void AzureServices(HostBuilderContext context, IServiceCollection services)
	{
		services.Configure<AzureStorageOptions>(
			context.Configuration.GetSection(nameof(AzureStorageOptions)));

		services.AddTransient(typeof(IStorageProvider<>), typeof(AzureStorageService<>));
		services.AddDbContext<Data.AppContext>(options => options.UseCosmos(
			connectionString: context.Configuration.GetOptions<AzureDatabaseOptions>().CosmosConnectionString,
			databaseName: AppName
		));

	}
}
