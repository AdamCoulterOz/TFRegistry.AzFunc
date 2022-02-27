using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Module;
public class Module : RegistryItem<RegistryItemVersion>
{
	[JsonPropertyName("provider")]
	public string Provider { get; set; }

	[JsonPropertyName("providers")]
	public List<string> Providers { get; set; }

	[JsonIgnore]
	public override string Address => $"{base.Address}/{Provider}";

	[JsonConstructor]
	public Module(string owner, string @namespace, string name, Uri source, DateTime published_at, List<RegistryItemVersion> versions, string provider, List<string> providers, string? description = null, Uri? logo_url = null)
		: base(owner, @namespace, name, source, published_at, versions, description, logo_url)
			=> (Versions, Provider, Providers) = (versions, provider, providers);
}
