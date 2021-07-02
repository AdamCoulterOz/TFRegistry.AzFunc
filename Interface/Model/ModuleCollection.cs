using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model
{
	public class ModuleCollection
	{
		[JsonPropertyName("modules")]
		public List<Module> Modules { get; set; }
	}
}