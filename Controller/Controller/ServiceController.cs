using System.Collections.Generic;
using System.Net.Http;
namespace PurpleDepot.Controller
{
	public class ServiceController
	{
		public static HttpResponseMessage ServiceDiscovery(HttpRequestMessage request)
		{
			var services = new Dictionary<string, string>()
			{
				["modules.v1"] = "/v1/modules",
				["providers.v1"] = "/v1/providers"
			};

			return request.CreateJsonResponse(services);
		}
	}
}
