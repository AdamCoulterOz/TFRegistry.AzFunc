using System;
using System.Reflection;

namespace PurpleDepot.Interface.Configuration
{
	public static class Initializer
	{
		public static void InitializeAttributes(this object item)
		{
			Type envVarAttribute = typeof(EnvironmentVariableAttribute);
			var initType = item.GetType().GetTypeInfo();
			var properties = initType.DeclaredProperties;
			foreach (var property in properties)
			{
				if (property.GetCustomAttribute(envVarAttribute, false) is not EnvironmentVariableAttribute envVar)
					continue;
				var value = Environment.GetEnvironmentVariable(envVar.Name);
				if (value is null && envVar.Required)
					throw new ArgumentException($"Required environment variable {envVar.Name} is not set.", envVar.Name);
				property.SetValue(item, value);
			}
		}
	}
}