namespace PurpleDepot.Interface.Model;

public abstract class Address
{
	public Address(string @namespace, string name)
		=> (Namespace, Name) = (@namespace, name);

	public string Namespace { get; }
	public string Name { get; }

	public override string ToString()
		=> $"{Namespace}/{Name}";
}
