using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Management.Automation;
using System;
using System.ComponentModel.DataAnnotations;

namespace PurpleDepot.Model
{
	public class Module
	{
		private Guid? _fileKey;

		[Key]
		[JsonPropertyName("namespace")]
		public string Namespace { get; set; }

		[Key]
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[Key]
		[JsonPropertyName("provider")]
		public string Provider { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonPropertyName("owner")]
		public string? Owner { get; set; }

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

		[JsonPropertyName("versions")]
		public List<VersionElement> Versions { get; set; }

		public Guid FileKey
		{
			get
			{
				if (!_fileKey.HasValue)
					_fileKey = Guid.NewGuid();
				return _fileKey.Value;
			}
			set
			{
				_fileKey = value;
			}
		}

		public Module(string @namespace, string name, string provider, string version)
		{
			Namespace = @namespace;
			Name = name;
			Provider = provider;
			Version = SemanticVersion.Parse(version).ToString();
			Versions = new List<VersionElement>();
		}
		public string FileName(string version) => $"{Namespace}-{Provider}-{Name}-{version}.zip";
	}
}