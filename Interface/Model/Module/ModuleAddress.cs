using FluentAssertions;

namespace PurpleDepot.Interface.Model.Module;

public class ModuleAddress : Address<Module>
{
	public string Provider { get; init; }

	public ModuleAddress(string @namespace, string name, string provider) : base(@namespace, name)
		=> Provider = provider;

	public override string ToString()
		=> $"{base.ToString()}/{Provider}";


	internal static ModuleAddress Parse(string value)
	{
		var parts = value.Split('/');
		parts.Should().HaveCount(3);
		return new ModuleAddress(parts[0], parts[1], parts[2]);
	}

	public override Module NewItem(string version)
		=> Module.New(this, version);

#nullable disable
	protected ModuleAddress() { }
}
