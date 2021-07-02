using System.Management.Automation;
using PurpleDepot.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PurpleDepot.Controller
{
	public static class ModelExtensions
	{
		public static SemanticVersion GetSemVer(this RegistryItemVersion mv) => new(mv.Version);

		public static RegistryItemVersion? Version(this RegistryItem item)
		{
			if (item.Versions.Count == 0)
				return null;
			var semVers = new List<SemanticVersion>();
			foreach (var version in item.Versions)
				semVers.Add(version.GetSemVer());
			semVers.Sort();
			var latestVersion = semVers.Last().ToString();
			return item.Version(latestVersion);
		}

		public static RegistryItemVersion? Version(this RegistryItem item, string version)
		{
			var versions = item.VersionsIndex();
			if (versions.ContainsKey(version))
				return versions[version];
			return null;
		}

		private static Dictionary<string, RegistryItemVersion> VersionsIndex(this RegistryItem item)
		{
			var versions = new Dictionary<string, RegistryItemVersion>();
			foreach (var v in item.Versions)
				versions.Add(v.Version, v);
			return versions;
		}

		public static bool HasVersion(this RegistryItem item, string version)
		{
			if (item.Versions.Count == 0) return false;
			if (version == "latest") return true;
			var versions = item.VersionsIndex();
			return versions.ContainsKey(version);
		}

		public static Guid? FileId(this RegistryItem item, string version)
			=> item.ResolveVersion(version)?.Id;

		public static RegistryItemVersion? ResolveVersion(this RegistryItem item, string version)
			=> version switch
			{
				"latest" => item.Version(),
				_ => item.Version(version)
			};

		public static void AddVersion(this RegistryItem item, RegistryItemVersion version)
		{
			item.Version = version;
			if (item.Version(version.Version) is not null)
				throw new Exception("Version already exists");
			item.Versions.Add(version);
		}
	}
}
