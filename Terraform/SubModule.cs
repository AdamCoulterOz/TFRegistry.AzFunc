using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdamCoulter.Terraform
{
	public class SubModule
	{
		[JsonPropertyName("path")]
		public string Path { get; set; }

		[JsonPropertyName("providers")]
		public List<Provider> Providers { get; set; }
		
		[JsonPropertyName("dependencies")]
		public List<Dependency> Dependencies { get; set; }

		public SubModule(string path)
		{
			Path = path;
			Providers = new List<Provider>();
			Dependencies = new List<Dependency>();
		}
	}
}