using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model
{
	public class ProviderVersionCollection
	{
		[JsonPropertyName("versions")]
		public List<ProviderVersion> Versions { get; set; }
	}
}