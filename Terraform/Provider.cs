using System.Text.Json.Serialization;

namespace AdamCoulter.Terraform
{
	public class Provider
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("namespace")]
		public string? Namespace { get; set; }

		[JsonPropertyName("source")]
		public string? Source { get; set; }
		
		[JsonPropertyName("version")]
		public string? Version { get; set; }

		public Provider(string name)
		{
			Name = name;
		}
	}
}