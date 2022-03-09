using System.Text.Json.Serialization;

namespace PurpleDepot.Core.Interface.Model.Module;

public class ModuleVersion : RegistryItemVersion
{
	public ModuleVersion(string version) : base(version) { }

	[JsonConstructor]
	public ModuleVersion(string version, Guid key) : base(version, key) { }
}
