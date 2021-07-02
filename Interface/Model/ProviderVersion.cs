using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model
{
	public class ProviderVersion : RegistryItemVersion
	{
		[JsonPropertyName("protocols")]
		public List<string> Protocols { get; set; }

		[JsonPropertyName("platforms")]
		public List<ProviderPlatform> Platforms { get; set; }
	}
}