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

		services.AddOptions<AzureStorageOptions>()
				.Configure(options => context.Configuration.GetSection(nameof(AzureStorageOptions)).Bind(options));

		services.AddOptions<AzureDatabaseOptions>()
				.Configure(options => context.Configuration.GetSection(nameof(AzureDatabaseOptions)).Bind(options));

		context.Configuration.AsEnumerable().ToList().ForEach(x => Console.WriteLine($"{x.Key} = {x.Value}"));

		var service = context.Configuration.GetValue<string>("Provider");
		Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!{service}!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		switch (service)
		{
			case "Azure":
				var storageOptions = context.Configuration.GetOptions<AzureStorageOptions>();
				Console.WriteLine(storageOptions.BlobClientUrl);
				services.AddTransient(typeof(IStorageProvider<>), typeof(AzureStorageService<>));

				var dbOptions = context.Configuration.GetOptions<AzureDatabaseOptions>();
				services.AddDbContext<AppContext>(options => options.UseCosmos(dbOptions.CosmosConnectionString, databaseName: AppName));

				break;
			default:
				services.AddScoped(typeof(IStorageProvider<>), typeof(MockStorageService<>));
				services.AddDbContext<AppContext>(options => options.UseInMemoryDatabase(AppName));
				break;
		}

		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
	}
}
