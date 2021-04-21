using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdamCoulter.Terraform
{
	public class Root
	{
		[JsonPropertyName("path")]
		public string? Path {get;set;}

		[JsonPropertyName("name")]
		public string? Name {get;set;}

		[JsonPropertyName("readme")]
		public string? ReadMe {get;set;}

		[JsonPropertyName("empty")]
		public bool? Empty {get;set;}

		[JsonPropertyName("inputs")]
		public List<Input> Inputs {get;set;}

		[JsonPropertyName("outputs")]
		public List<Output> Outputs {get;set;}

		[JsonPropertyName("dependencies")]
		public List<Dependency> Dependencies { get; set; }

		[JsonPropertyName("providers")]
		public List<Provider> Providers { get; set; }

		[JsonPropertyName("resources")]
		public List<Resource> Resources {get;set;}

		public Root()
		{
			Providers = new List<Provider>();
			Dependencies = new List<Dependency>();
			Inputs = new List<Input>();
			Outputs = new List<Output>();
			Resources = new List<Resource>();
		}
	}
}