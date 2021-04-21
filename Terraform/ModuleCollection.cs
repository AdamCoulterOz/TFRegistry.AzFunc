using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdamCoulter.Terraform
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