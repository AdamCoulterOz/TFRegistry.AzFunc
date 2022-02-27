namespace PurpleDepot.Interface.Model.Module;

public class ModuleAddress : Address
{
	public string Provider { get; set; }
	public ModuleAddress(string @namespace, string name, string provider) : base(@namespace, name)
		=> Provider = provider;

	public override string ToString()
		=> $"{base.ToString()}/{Provider}";
}
