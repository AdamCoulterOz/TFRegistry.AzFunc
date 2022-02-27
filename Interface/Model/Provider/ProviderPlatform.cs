using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Provider;
public class ProviderPlatform
{
	[JsonPropertyName("os")]
	public string OS { get; set; }

	[JsonPropertyName("arch")]
	public string Arch { get; set; }

	[JsonConstructor]
	public ProviderPlatform(string os, string arch)
		=> (OS, Arch) = (os, arch);
}
