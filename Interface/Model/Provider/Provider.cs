using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Provider;
public class Provider : RegistryItem
{
	[JsonPropertyName("alias")]
	public string? Alias { get; set; }

	public override ProviderAddress Address => GetAddress(Namespace, Name);

	public List<ProviderVersion> Versions { get; init; }
	public override List<RegistryItemVersion> GetVersions() => Versions.ToList<RegistryItemVersion>();

	public static ProviderAddress GetAddress(string @namespace, string name)
		=> new ProviderAddress(@namespace, name);

	public static ProviderAddress GetAddress(string value)
		=> ProviderAddress.Parse(value);

	[JsonConstructor]
	public Provider(string id, string @namespace, string name, List<ProviderVersion> versions, DateTime published_at, string? alias = null, string? owner = null, string? description = null, Uri? source = null, Uri? logo_url = null)
		: base(id, @namespace, name, published_at, owner, description, source, logo_url)
			=> (Alias, Versions) = (alias, versions);

	protected Provider(ProviderAddress id, string version) : base(id)
		=> Versions = new List<ProviderVersion> { new ProviderVersion(version) };

	public static Provider New(ProviderAddress id, string version)
		=> new Provider(id, version);

	protected override void AddSpecificVersion(RegistryItemVersion version)
	{
		if(version is ProviderVersion providerVersion)
			Versions.Add(providerVersion);
		else
			throw new ArgumentException($"{nameof(version)} must be of type {nameof(ProviderVersion)}");
	}

#nullable disable
	protected Provider() : base()
	{ }
}
