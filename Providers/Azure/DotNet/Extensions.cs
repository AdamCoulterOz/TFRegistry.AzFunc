using Microsoft.Extensions.Configuration;

namespace PurpleDepot.Providers.Azure
{
	public static class Extensions{
		public static T GetOptions<T>(this IConfiguration configuration)
			=> configuration.GetValue<T>(nameof(T));
	}
}