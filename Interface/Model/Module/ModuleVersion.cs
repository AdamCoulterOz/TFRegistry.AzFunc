namespace PurpleDepot.Interface.Model.Module;

public class ModuleVersion : RegistryItemVersion
{
	public ModuleVersion(string version) : base(version) { }
	public ModuleVersion(string version, Guid key) : base(version, key) { }
}
