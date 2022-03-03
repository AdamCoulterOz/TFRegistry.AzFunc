using PurpleDepot.Interface.Routes;

namespace PurpleDepot.Interface.Model.Provider;

public class ProviderRoutes : IRoutes
{
	public static string RootName => "providers.v1";
	public static string RootPath => Root;


	public const string Root = "v1/providers";
	private const string Common = $"{Root}/{{namespace}}/{{name}}";
	private const string CommonVersion = $"{Common}/{{version}}";


	public const string Versions = $"{Common}/versions";
	public const string Download = $"{Common}/download";
	public const string DownloadVersion = $"{CommonVersion}/download";
	public const string Latest = $"{Common}";
	public const string Specific = $"{CommonVersion}";
	public const string Ingest = $"{CommonVersion}/upload";
}
