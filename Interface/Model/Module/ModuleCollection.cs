using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Module;
public class ModuleCollection
{
	[JsonPropertyName("modules")]
	public List<Module> Modules { get; set; }

	[JsonConstructor]
	public ModuleCollection(List<Module> modules)
		=> Modules = modules;
}
