namespace PurpleDepot.Interface.Model;

public abstract class Address<T>
	where T: RegistryItem<T>
{
	public string Namespace { get; }
	public string Name { get; }
	public Address(string @namespace, string name)
		=> (Namespace, Name) = (@namespace, name);

	public override string ToString()
		=> $"{Namespace}/{Name}";

#nullable disable
	protected Address() { }

	public abstract T NewItem(string version);
}
