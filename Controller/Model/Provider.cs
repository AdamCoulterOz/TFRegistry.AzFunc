using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Management.Automation;
using System.Text.Json.Serialization;
using System.Linq;

namespace PurpleDepot.Model
{
	public class Provider
	{
		[Key]
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("namespace")]
		public string Namespace { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("owner")]
		public string? Owner { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("published_at")]
		public DateTime PublishedAt { get; set; }

		[JsonPropertyName("downloads")]
		public int? Downloads { get; set; }

		[JsonPropertyName("versions")]
		public List<ProviderVersion> Versions { get; set; }

		[JsonPropertyName("latest_version")]
		public string LatestVersion { get => Version()!.Version; }

		public ProviderVersion? Version()
		{
			if (Versions.Count == 0)
				return null;
			var semVers = new List<SemanticVersion>();
			foreach (var version in Versions)
				semVers.Add(version.GetSemVer());
			semVers.Sort();
			var latestVersion = semVers.Last().ToString();
			return Version(latestVersion);
		}

		public ProviderVersion? Version(string version)
		{
			var versions = VersionsIndex();
			if (versions.ContainsKey(version))
				return versions[version];
			return null;
		}

		private Dictionary<string, ProviderVersion> VersionsIndex()
		{
			var versions = new Dictionary<string, ProviderVersion>();
			foreach (var v in Versions)
				versions.Add(v.Version, v);
			return versions;
		}

		public bool HasVersion(string version)
		{
			if (Versions.Count == 0) return false;
			if (version == "latest") return true;
			var versions = VersionsIndex();
			return versions.ContainsKey(version);
		}

		public Provider(Guid Id, string Namespace, string Name, DateTime PublishedAt)
		{
			this.Id = Id;
			this.Namespace = Namespace;
			this.Name = Name;
			this.PublishedAt = PublishedAt;
			this.Versions = new List<ProviderVersion>();
		}

		public Provider(string @namespace, string name)
		{
			Id = Guid.NewGuid();
			PublishedAt = DateTime.Now;
			Namespace = @namespace;
			Name = name;
			Versions = new List<ProviderVersion>();
		}
		public Guid? FileId(string version)
		{
			return ResolveVersion(version)?.Id;
		}
		public string FileName(string version)
		{
			var versionResolved = ResolveVersion(version);
			if (versionResolved is null)
				throw new Exception("Couldn't resolve version of Provider.");
			return $"{Namespace}-{Name}-{versionResolved.Version}.zip";
		}

		public ProviderVersion? ResolveVersion(string version)
		{
			return version switch
			{
				"latest" => Version(),
				_ => Version(version)
			};
		}

		internal void AddVersion(string version)
		{
			if (Version(version) is not null)
				throw new Exception("Version already exists.");
			Versions.Add(new ProviderVersion(version));
		}
	}
}