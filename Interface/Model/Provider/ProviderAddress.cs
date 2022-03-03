using FluentAssertions;

namespace PurpleDepot.Interface.Model.Provider;
public class ProviderAddress : Address
{
	public ProviderAddress(string @namespace, string name)
		: base(@namespace, name) { }

	protected ProviderAddress() { }

	internal static ProviderAddress Parse(string value)
	{
		var parts = value.Split('/');
		parts.Should().HaveCount(2);
		return new ProviderAddress(parts[0], parts[1]);
	}

	public override bool ItemIsValidType<T>(T item)
		=> (item as Provider) != null;
}
