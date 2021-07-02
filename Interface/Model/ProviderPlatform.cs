using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model
{
	public class ProviderPlatform
	{
		[JsonPropertyName("os")]
		public string OS { get; set; }

		[JsonPropertyName("arch")]
		public string Arch { get; set; }
	}
}