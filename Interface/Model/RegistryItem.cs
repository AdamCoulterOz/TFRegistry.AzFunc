using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model;
public abstract class RegistryItem<T> where T: RegistryItemVersion
{
	public string Id => $"{Address}/{Version}";

	[JsonPropertyName("owner")]
	public string Owner { get; set; }

	[JsonPropertyName("namespace")]
	public string Namespace { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>Latest version</summary>
	public T? Version => LatestVersion;

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("source")]
	public Uri Source { get; set; }

	[JsonPropertyName("published_at")]
	public DateTime PublishedAt { get; set; }

	[JsonPropertyName("versions")]
	public virtual List<T> Versions { get; set; }

	[JsonPropertyName("logo_url")]
	public Uri? LogoUrl { get; set; }

	[JsonIgnore]
	public virtual string Address => $"{Namespace}/{Name}";

	[JsonConstructor]
	public RegistryItem(string owner, string @namespace, string name, Uri source, DateTime published_at, List<T> versions, string? description = null, Uri? logo_url = null)
		=> (Owner, Namespace, Name, Description, Source, PublishedAt, Versions, LogoUrl) = (owner, @namespace, name, description, source, published_at, versions, logo_url);

	/// <summary>Gets latest version</summary>
	public T? LatestVersion => Versions.Max();

	/// <summary>Gets specific version</summary>
	public T? GetVersion(string version)
		=> Versions.Where(v => v.Version == version).FirstOrDefault();

	public void AddVersion(T version)
	{
		if (GetVersion(version.Version) is not null)
			throw new Exception("Version already exists");
		Versions.Add(version);
	}
}
