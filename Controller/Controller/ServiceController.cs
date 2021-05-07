using System.Collections.Generic;
using System.Net.Http;
namespace PurpleDepot.Controller
{
	public class ServiceController
	{
		public static HttpResponseMessage ServiceDiscovery(HttpRequestMessage request, string moduleV1path)
		{
			var services = new Dictionary<string, string>()
			{
				["modules.v1"] = moduleV1path
			};

			return request.CreateJsonResponse(services);
		}
	}
}
