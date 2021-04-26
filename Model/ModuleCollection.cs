using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurpleDepot.Model
{
	public class ModuleCollection
	{
		[JsonPropertyName("modules")]
		public List<Module> Modules { get; set; }

		public ModuleCollection()
		{
			Modules = new List<Module>();
		}
	}
}