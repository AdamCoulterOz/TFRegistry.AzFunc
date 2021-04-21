using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Management.Automation;
using System;

namespace AdamCoulter.Terraform
{
	public class Module
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("owner")]
		public string? Owner { get; set; }

		[JsonPropertyName("namespace")]
		public string? Namespace { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("version")]
		public SemanticVersion? Version { get; set; }

		[JsonPropertyName("provider")]
		public string? Provider { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("source")]
		public Uri? Source { get; set; }

		[JsonPropertyName("tag")]
		public string? Tag { get; set; }

		[JsonPropertyName("published_at")]
		public DateTime PublishedAt { get; set; }

		[JsonPropertyName("downloads")]
		public int? Downloads { get; set; }

		[JsonPropertyName("verified")]
		public bool? Verified { get; set; }

		[JsonPropertyName("root")]
		public Root? Root { get; set; }

		[JsonPropertyName("submodules")]
		public List<SubModule> SubModules { get; set; }

		[JsonPropertyName("examples")]
		public List<Example> Examples {get;set;}

		[JsonPropertyName("providers")]
		public List<Provider> Providers { get; set; }

		[JsonPropertyName("versions")]
		public List<VersionElement> Versions { get; set; }

		public Module()
		{
			SubModules = new List<SubModule>();
			Examples = new List<Example>();
			Providers = new List<Provider>();
			Versions = new List<VersionElement>();
		}
	}
}