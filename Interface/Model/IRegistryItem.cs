namespace PurpleDepot.Interface.Model;

public interface IRegistryItem
{
	public Address Id { get; }
	public string Owner { get; set; }
	public string Namespace { get; set; }
	public string Name { get; set; }
	public RegistryItemVersion Version { get; }
	public string? Description { get; set; }
	public Uri Source { get; set; }
	public DateTime PublishedAt { get; set; }
	public Uri? LogoUrl { get; set; }
	public Address Address { get; }
	public void AddVersion(RegistryItemVersion version);
	public RegistryItemVersion? GetVersion(string? version);
	public bool HasVersion(string version);
	string GetFileKey(RegistryItemVersion? version);
}
