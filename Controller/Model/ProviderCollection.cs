using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PurpleDepot.Model
{
	public class ProviderCollection
	{
		[JsonPropertyName("modules")]
		public List<Provider> Providers { get; set; }

		public ProviderCollection()
		{
			Providers = new List<Provider>();
		}
	}
}