using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model
{
	public class Module : RegistryItem
	{
		[JsonPropertyName("provider")]
		public string Provider { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonPropertyName("published_at")]
		public DateTime PublishedAt { get; set; }

		[JsonPropertyName("versions")]
		public List<RegistryItemVersion> Versions { get; set; }
	}
}