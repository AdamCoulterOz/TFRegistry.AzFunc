using System.Text.Json.Serialization;

namespace PurpleDepot.Core.Interface.Model.Module;
public class ModuleCollection
{
	[JsonPropertyName("modules")]
	public List<Module> Modules { get; }

	[JsonConstructor]
	public ModuleCollection(List<Module> modules)
		=> Modules = modules;
}
