using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Provider;
public class Provider : RegistryItem<ProviderVersion>
{
	[JsonPropertyName("alias")]
	public string Alias { get; set; }

	[JsonConstructor]
	public Provider(string owner, string @namespace, string name, Uri source, DateTime published_at, List<ProviderVersion> versions, string alias, string? description = null, Uri? logo_url = null)
		: base(owner, @namespace, name, source, published_at, versions, description, logo_url)
			=> Alias = alias;
}
