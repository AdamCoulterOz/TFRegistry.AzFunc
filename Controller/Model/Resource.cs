using System.Text.Json.Serialization;

namespace PurpleDepot.Model
{
	public class Resource
	{
		[JsonPropertyName("name")]
		public string Name {get;set;}

		[JsonPropertyName("type")]
		public string Type {get;set;}

		public Resource(string name, string type){
			Name = name;
			Type = type;
		}
	}
}