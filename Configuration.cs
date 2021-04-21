namespace AdamCoulter.Terraform
{
	public static class Configuration
	{
		public static string StorageAccount => GetConfigValue("STORAGE_ACCOUNT");
		public static string BlobContainer => GetConfigValue("BLOB_CONTAINER");

		private static string GetConfigValue(string key)
		{
			var value = System.Environment.GetEnvironmentVariable(key);
			if (value is null)
				throw new System.Exception($"Required environment variable {key} not set.");
			return value;
		}
	}
}