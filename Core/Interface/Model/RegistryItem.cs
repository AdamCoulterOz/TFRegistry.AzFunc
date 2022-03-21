using System.Text.Json.Serialization;

namespace PurpleDepot.Core.Interface.Model;

public abstract class RegistryItem<T>
	where T: RegistryItem<T>
{
	[JsonPropertyName("id")]
	public string Id { get; init; }

	[JsonPropertyName("namespace")]
	public string Namespace { get; init; }

	[JsonPropertyName("name")]
	public string Name { get; init; }

	public abstract List<RegistryItemVersion> GetVersions();

	/// <summary>Latest version</summary>
	public RegistryItemVersion Version => GetVersions().Max()!;

	[JsonPropertyName("owner")]
	public string? Owner { get; set; }

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("source")]
	public Uri? Source { get; set; }

	[JsonPropertyName("published_at")]
	public DateTime PublishedAt { get; init; }

	[JsonPropertyName("logo_url")]
	public Uri? LogoUrl { get; set; }

	[JsonIgnore]
	protected abstract Address<T> Address { get; }

	protected RegistryItem(Address<T> id)
		: this(
			id.ToString(),
			id.Namespace,
			id.Name,
			DateTime.UtcNow)
	{ }

	[JsonConstructor]
	public RegistryItem(string id, string @namespace, string name, DateTime published_at,
		string? owner = null, string? description = null, Uri? source = null, Uri? logo_url = null)
	{
		(Id, Owner, Namespace, Name, Description, Source, PublishedAt, LogoUrl) = (id, owner, @namespace, name, description, source, published_at, logo_url);
	}

	public bool HasVersion(string version)
		=> GetVersion(version) is not null;

	/// <summary>Gets specific version</summary>
	public RegistryItemVersion? GetVersion(string? version = null)
	{
		if (version is null)
			return Version;
		return GetVersions().Where(v => v.Version == version).FirstOrDefault();
	}

	public RegistryItemVersion AddVersion(RegistryItemVersion version)
	{
		if (GetVersion(version.Version) is not null)
			throw new Exception("Version already exists");
		return AddSpecificVersion(version);
	}

	public RegistryItemVersion AddVersion(string version)
	{
		if (GetVersion(version) is not null)
			throw new Exception("Version already exists");
		return AddSpecificVersion(version);
	}

	protected abstract RegistryItemVersion AddSpecificVersion(RegistryItemVersion version);
	protected abstract RegistryItemVersion AddSpecificVersion(string version);

	public string GetFileKey(RegistryItemVersion? version = null)
	{
		if (version is null)
			version = Version;
		return $"{Address}/{version.Key}-{version.Version}.zip";
	}

#nullable disable
	protected RegistryItem() { }
}
