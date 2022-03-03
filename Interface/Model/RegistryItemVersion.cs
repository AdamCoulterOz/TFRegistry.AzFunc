using System.Text.Json.Serialization;
using SemVersion;

namespace Interface.Model;

public class RegistryItemVersion : SemanticVersion
{
	[JsonPropertyName("version")]
	public string Version { get; set; }

	[JsonPropertyName("key")]
	public Guid Key { get; set; }

	[JsonConstructor]
	public RegistryItemVersion(string version, Guid key) : base(GetMajor(version), GetMinor(version), GetPatch(version), GetPrerelease(version), GetBuild(version))
		=> (Version, Key) = (version, key);

	public RegistryItemVersion(string version) : base(GetMajor(version), GetMinor(version), GetPatch(version), GetPrerelease(version), GetBuild(version))
		=> (Version, Key) = (version, Guid.NewGuid());

	private static int? GetMajor(string version)
		=> ToSemVer(version).Major;

	private static int? GetMinor(string version)
		=> ToSemVer(version).Minor;

	private static int? GetPatch(string version)
		=> ToSemVer(version).Patch;

	private static string GetBuild(string version)
		=> ToSemVer(version).Build;

	private static string GetPrerelease(string version)
		=> ToSemVer(version).Prerelease;

	private static SemanticVersion ToSemVer(string version)
		=> Parse(version);


#nullable disable
	protected RegistryItemVersion() : base(null, null, null) { }
}
