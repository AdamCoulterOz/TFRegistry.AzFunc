using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PurpleDepot.Model
{
	public class VersionElement
	{
		[Key]
		[JsonPropertyName("version")]
		public string Version { get; set; }

		[Key]
		[JsonIgnore]
		public Module Module { get; set; }
	}
}