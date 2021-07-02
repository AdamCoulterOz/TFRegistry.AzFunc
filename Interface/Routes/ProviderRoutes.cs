namespace PurpleDepot.Interface.Routes
{
	public static class ProviderRoutes {
		public const string Root = "v1/providers";
		private const string Common = $"{Root}/{{namespace}}/{{name}}";
		private const string CommonVersion = $"{Common}/{{version}}";


		public const string GetVersions = $"{Common}/versions";
		public const string GetDownload = $"{Common}/download";
		public const string GetDownloadVersion = $"{CommonVersion}/download";
		public const string GetLatest = $"{Common}";
		public const string GetSpecific = $"{CommonVersion}";
		public const string PostIngest = $"{Root}";
	}
}