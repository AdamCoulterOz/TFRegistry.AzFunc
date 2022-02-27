using System.Text.Json.Serialization;
using FluentAssertions;

namespace PurpleDepot.Interface.Model;

public abstract class RegistryItem<V> : IRegistryItem
	where V : RegistryItemVersion
{
	public Address Id => Address;

	[JsonPropertyName("owner")]
	public string Owner { get; set; }

	[JsonPropertyName("namespace")]
	public string Namespace { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>Latest version</summary>
	public V Version => Versions.Max()!;
	RegistryItemVersion IRegistryItem.Version => Version;

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("source")]
	public Uri Source { get; set; }

	[JsonPropertyName("published_at")]
	public DateTime PublishedAt { get; set; }

	[JsonPropertyName("versions")]
	public virtual List<V> Versions { get; set; }

	[JsonPropertyName("logo_url")]
	public Uri? LogoUrl { get; set; }

	[JsonIgnore]
	public abstract Address Address { get; }

	[JsonConstructor]
	public RegistryItem(string owner, string @namespace, string name, Uri source, DateTime published_at, List<V> versions, string? description = null, Uri? logo_url = null)
	{
		(Owner, Namespace, Name, Description, Source, PublishedAt, Versions, LogoUrl) = (owner, @namespace, name, description, source, published_at, versions, logo_url);
		Versions.Should().NotBeEmpty(because: "all registry items must have at least one version in order to exist");
	}

	public bool HasVersion(string version)
		=> GetVersion(version) is not null;

	/// <summary>Gets specific version</summary>
	public V? GetVersion(string? version = null)
	{
		if(version is null)
			return Version;
		return Versions.Where(v => v.Version == version).FirstOrDefault();
	}
	RegistryItemVersion? IRegistryItem.GetVersion(string? version) => GetVersion(version);

	public void AddVersion(V version)
	{
		if (GetVersion(version.Version) is not null)
			throw new Exception("Version already exists");
		Versions.Add(version);
	}
	void IRegistryItem.AddVersion(RegistryItemVersion version) => AddVersion((V)version);

	public string GetFileKey(RegistryItemVersion? version = null)
	{
		if(version is null)
			version = Version;
		return $"{Address}/{version.Key}-{version.Version}.zip";
	}
}
