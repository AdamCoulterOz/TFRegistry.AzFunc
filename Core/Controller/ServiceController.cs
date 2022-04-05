using PurpleDepot.Core.Interface.Model;

namespace PurpleDepot.Core.Controller;
public class ServiceController
{
	private static readonly Dictionary<string, string> Services;
	static ServiceController()
	{
		Services = new Dictionary<string, string>();
		var routableServices = AppDomain.CurrentDomain.GetAssemblies().SelectMany(
						assembly => assembly.GetTypes().Where(
							type => type.GetInterfaces().Contains(typeof(IRoutes))
								&& !type.IsAbstract));

		foreach (var service in routableServices)
		{
			Services.Add(
				service.GetStaticProperty<string>(nameof(IRoutes.RootName))!,
				$"/{service.GetStaticProperty<string>(nameof(IRoutes.RootPath))!}/"
			);
		}
	}

	protected static async Task<ControllerResult> ServiceDiscovery()
		=> await Task.Run(() => ControllerResult.NewJson(Services));
}