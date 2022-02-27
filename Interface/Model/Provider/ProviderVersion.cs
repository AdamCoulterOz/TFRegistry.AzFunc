using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Provider;
public class ProviderVersion : RegistryItemVersion
{
	[JsonPropertyName("protocols")]
	public List<string> Protocols { get; set; }

	[JsonPropertyName("platforms")]
	public List<ProviderPlatform> Platforms { get; set; }

	[JsonConstructor]
	public ProviderVersion(string version, List<string> protocols, List<ProviderPlatform> platforms)
		: base(version)
			=> (Protocols, Platforms) = (protocols, platforms);
}
