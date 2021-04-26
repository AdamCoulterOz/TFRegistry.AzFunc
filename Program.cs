using Microsoft.Extensions.Hosting;

namespace TFRegistry.AzFunc
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .Build();
            host.Run();
        }
    }
}