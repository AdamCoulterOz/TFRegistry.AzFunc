using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Module;
public class Module : RegistryItem
{
	[JsonPropertyName("provider")]
	public string Provider { get; set; }

	[JsonPropertyName("providers")]
	public List<string> Providers { get; init; }

	public List<ModuleVersion> Versions { get; init; }

	[JsonIgnore]
	public override ModuleAddress Address => GetAddress(Namespace, Name, Provider);

	public override List<RegistryItemVersion> GetVersions() => Versions.ToList<RegistryItemVersion>();

	public static ModuleAddress GetAddress(string @namespace, string name, string provider)
		=> new ModuleAddress(@namespace, name, provider);

	public static ModuleAddress GetAddress(string value)
	=> ModuleAddress.Parse(value);

	public static Module New(ModuleAddress id, string version)
		=> new Module(id, version);

	protected override void AddSpecificVersion(RegistryItemVersion version)
	{
		if (version is ModuleVersion moduleVersion)
			Versions.Add(moduleVersion);
		else
			throw new ArgumentException($"{nameof(version)} must be of type {nameof(ModuleVersion)}");
	}

	protected Module(ModuleAddress id, string version)
	: base(id)
	{
		Provider = id.Provider;
		Providers = new List<string> { Provider };
		Versions = new List<ModuleVersion> { new ModuleVersion(version) };
	}

	[JsonConstructor]
	public Module(string id, string @namespace, string name, List<ModuleVersion> versions, string provider,
		List<string> providers, DateTime published_at, string? owner = null, string? description = null, Uri? source = null, Uri? logo_url = null)
		: base(id, @namespace, name, published_at, owner, description, source, logo_url)
			=> (Provider, Providers, Versions) = (provider, providers, versions);

#nullable disable
	protected Module() : base() { }
}
