using Microsoft.Extensions.Configuration;

namespace Azure;

public static class Extensions{
	public static T GetOptions<T>(this IConfiguration configuration)
		=> configuration.GetValue<T>(nameof(T));
}