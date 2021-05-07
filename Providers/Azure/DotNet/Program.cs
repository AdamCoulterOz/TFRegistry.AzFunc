using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PurpleDepot.Data;
using PurpleDepot.Interface.Storage;
using PurpleDepot.Providers.Storage.Azure;

namespace PurpleDepot.Providers.Azure
{
	public class Program
	{
        const string ServiceInjection = "SERVICE_INJECTION";
		public static void Main()
		{
            var service = Environment.GetEnvironmentVariable(ServiceInjection);
            Action<HostBuilderContext, IServiceCollection> delegateServices = service switch
            {
                "Azure" => AzureServices,
                "Dev" => DevServices,
                null => DevServices,
                _ => throw new ArgumentException($"Invalid {ServiceInjection} value {service}.", paramName: ServiceInjection)                
            };

			var host = new HostBuilder()
				.ConfigureFunctionsWorkerDefaults()
				.ConfigureServices(delegateServices)
				.Build();
			host.Run();
		}

		static void DevServices(HostBuilderContext _, IServiceCollection services)
		{
			services.AddTransient<IStorageProvider, MockStorageService>();
			services.AddDbContext<ModuleContext>(options => options.UseInMemoryDatabase("PurpleDepot"));
		}

        static void AzureServices(HostBuilderContext _, IServiceCollection services)
		{
			services.AddTransient<IStorageProvider, AzureStorageService>();
            services.AddDbContext<ModuleContext>(options => options.UseCosmos(
                    accountEndpoint: "https://testcosmos.documents.azure.com:443/",
                    accountKey: "SuperSecretKey", databaseName: "dbName")
                );
		}
	}
}