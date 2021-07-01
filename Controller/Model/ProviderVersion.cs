using System;
using System.Management.Automation;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace PurpleDepot.Model
{
	[Owned]
	public class ProviderVersion
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }
		
		[JsonPropertyName("version")]
		public string Version { get; set; }
		public SemanticVersion GetSemVer() => new(Version);

		public ProviderVersion(string version)
		{
			Version = version;
			Id = Guid.NewGuid();
		}
	}
}