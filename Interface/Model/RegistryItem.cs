using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model
{
	public abstract class RegistryItem
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("namespace")]
		public string Namespace { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("version")]
		public RegistryItemVersion Version { get; set; }

		[JsonPropertyName("versions")]
		public virtual List<RegistryItemVersion> Versions { get; set; }
	}
}