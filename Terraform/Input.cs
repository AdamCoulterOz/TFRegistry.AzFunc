namespace AdamCoulter.Terraform
{
	public class Input
	{
		public string Name {get;set;}
		public string Type {get;set;}
		public string? Description {get;set;}
		public string? Default {get;set;}
		public bool Required {get;set;}

		public Input(string name, string type)
		{
			Name = name;
			Type = type;
		}
	}
}