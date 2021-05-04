using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Management.Automation;

namespace PurpleDepot.Model
{
	public class VersionElement
	{
		[JsonPropertyName("version")]
		public string Version { get; set; }

		public SemanticVersion SemVer
		{
			get => new(Version);
			set => Version = value.ToString();
		}

		[JsonPropertyName("root")]
		public Root? Root { get; set; }

		[JsonPropertyName("submodules")]
		public List<SubModule>? SubModules { get; set; }

		public VersionElement(SemanticVersion version)
		{
			Version = version.ToString();
		}

		public VersionElement(string version)
		{
			Version = version;
		}
	}
}