using Azure.Database;
using Azure.Storage;
using Controller.Data;
using Interface.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AppContext = Controller.Data.AppContext;

namespace Azure;

public static class Program
{
	private const string AppName = "PurpleDepot";

	public static void Main()
	{
		var host = new HostBuilder()
			.ConfigureFunctionsWorkerDefaults()
			.ConfigureServices(ConfigureServices)
			.Build();

		host.Run();
	}

	private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		context.Configuration = new ConfigurationBuilder()
			.AddEnvironmentVariables(AppName)
			.Build();

		var service = context.Configuration.GetValue<string>("Provider");
		switch (service)
		{
			case "Azure":
				AzureServices(context, services);
				break;
			default:
				DevServices(services);
				break;
		}

		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
	}

	private static void DevServices(IServiceCollection services)
	{
		services.AddScoped(typeof(IStorageProvider<>), typeof(MockStorageService<>));
		services.AddDbContext<AppContext>(options => options.UseInMemoryDatabase(AppName));
	}

	private static void AzureServices(HostBuilderContext context, IServiceCollection services)
	{
		services.Configure<AzureStorageOptions>(
			context.Configuration.GetSection(nameof(AzureStorageOptions)));

		services.AddTransient(typeof(IStorageProvider<>), typeof(AzureStorageService<>));
		services.AddDbContext<AppContext>(options => options.UseCosmos(
			context.Configuration.GetOptions<AzureDatabaseOptions>().CosmosConnectionString, AppName
		));
	}
}
