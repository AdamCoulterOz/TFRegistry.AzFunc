using System.Text.Json.Serialization;

namespace PurpleDepot.Core.Interface.Model.Module;
public class Module : RegistryItem<Module>
{
	[JsonPropertyName("provider")]
	public string Provider { get; set; }

	[JsonPropertyName("providers")]
	public List<string> Providers { get; }

	public List<ModuleVersion> Versions { get; }

	[JsonIgnore]
	protected override ModuleAddress Address => GetAddress(Namespace, Name, Provider);

	public override List<RegistryItemVersion> GetVersions() => Versions.ToList<RegistryItemVersion>();

	private static ModuleAddress GetAddress(string @namespace, string name, string provider)
		=> new ModuleAddress(@namespace, name, provider);

	public static Module New(ModuleAddress id, string version)
		=> new Module(id, version);

	protected override ModuleVersion AddSpecificVersion(RegistryItemVersion version)
	{
		if (version is ModuleVersion moduleVersion)
		{
			Versions.Add(moduleVersion);
			return moduleVersion;
		}
		throw new ArgumentException($"{nameof(version)} must be of type {nameof(ModuleVersion)}");
	}

	protected override ModuleVersion AddSpecificVersion(string version)
		=> AddSpecificVersion(new ModuleVersion(version));

	protected override string GetTypeName() => "modules";

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
	protected Module()
	{ }
}
