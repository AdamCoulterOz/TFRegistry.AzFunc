using System;
using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model
{
	public class RegistryItemVersion
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }
	}
}