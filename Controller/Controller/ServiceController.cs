using PurpleDepot.Interface.Routes;

namespace PurpleDepot.Controller;
public class ServiceController
{
	private static Dictionary<string, string> Services;
	static ServiceController()
	{
		Services = new Dictionary<string, string>();
		var routableServices = AppDomain.CurrentDomain.GetAssemblies().SelectMany(
						assembily => assembily.GetTypes().Where(
							type => type.GetInterfaces().Contains(typeof(IRoutes))
								&& !type.IsAbstract));

		foreach (var service in routableServices)
		{
			Services.Add(
				service.GetStaticProperty<string>(nameof(IRoutes.RootName))!,
				service.GetStaticProperty<string>(nameof(IRoutes.RootPath))!
			);
		}
	}
	public static async Task<HttpResponseMessage> ServiceDiscovery(HttpRequestMessage request)
		=> await Task.Run(() => request.CreateJsonResponse(Services));
}

public static class RandomExtensions
{
	public static T? GetStaticProperty<T>(this Type type, string name)
		=> (T?)type.GetProperty(name)?.GetValue(null);
}