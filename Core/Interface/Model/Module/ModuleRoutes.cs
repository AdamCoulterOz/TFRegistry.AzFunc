namespace Interface.Model.Module;

public class ModuleRoutes : IRoutes
{
	public static string RootName => "modules.v1";
	public static string RootPath => Root;

	private const string Root = "v1/modules";

	private const string Common = $"{Root}/{{namespace}}/{{name}}/{{provider}}";
	private const string CommonVersion = $"{Common}/{{version}}";


	public const string Versions = $"{Common}/versions";
	public const string Download = $"{Common}/download";
	public const string DownloadVersion = $"{CommonVersion}/download";
	public const string Latest = $"{Common}";
	public const string Version = $"{CommonVersion}";
	public const string Ingest = $"{CommonVersion}/upload";
}