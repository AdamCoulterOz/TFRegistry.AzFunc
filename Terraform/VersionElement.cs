using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Management.Automation;

namespace AdamCoulter.Terraform
{
	public class VersionElement
	{
		[JsonPropertyName("version")]
		public SemanticVersion Version { get; set; }

		[JsonPropertyName("root")]
		public Root? Root { get; set; }
		
		[JsonPropertyName("submodules")]
		public List<SubModule>? SubModules { get; set; }

		public VersionElement(SemanticVersion version)
		{
			Version = version;
		}
	}
}