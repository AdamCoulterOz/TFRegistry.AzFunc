
using System.Text.Json.Serialization;
using SemVersion;

namespace PurpleDepot.Interface.Model;
public class RegistryItemVersion : SemanticVersion
{
	[JsonPropertyName("version")]
	public string Version { get; set; }

	[JsonConstructor]
	public RegistryItemVersion(string version): base(GetMajor(version), GetMinor(version), GetPatch(version), GetPrerelease(version), GetBuild(version))
		=> Version = version;

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
		=> SemanticVersion.Parse(version);
}
