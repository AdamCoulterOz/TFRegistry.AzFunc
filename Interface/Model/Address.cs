using ObjectsComparer;

namespace PurpleDepot.Interface.Model;

public abstract class Address
{
	public abstract bool ItemIsValidType<T>(T item) where T : RegistryItem;
	public string Namespace { get; }
	public string Name { get; }
	public Address(string @namespace, string name)
		=> (Namespace, Name) = (@namespace, name);

	public override string ToString()
		=> $"{Namespace}/{Name}";

	public bool MatchesItem<T>(T item)
		where T : RegistryItem
	{
		if (ItemIsValidType<T>(item))
			return Comparer.Equals(this, item.Address);
		throw new Exception($"{item.GetType().Name} is not compatible with {GetType().Name}");
	}

#nullable disable
	protected Address() { }
}
