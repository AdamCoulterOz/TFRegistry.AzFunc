using System.Text.Json.Serialization;

namespace PurpleDepot.Model
{
	public class Output
	{
		[JsonPropertyName("name")]
		public string Name {get;set;}

		[JsonPropertyName("type")]
		public string? Type {get;set;}

		[JsonPropertyName("description")]
		public string? Description {get;set;}

		public Output(string name)
		{
			Name = name;
		}
	}
}