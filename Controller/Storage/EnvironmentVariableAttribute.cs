using System;

namespace PurpleDepot.Controller
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EnvironmentVariableAttribute : Attribute
	{
		public string Name { get; set; }
		public bool Required { get; set; } = false;
		public EnvironmentVariableAttribute(string name, bool required = false)
		{
			Name = name;
			Required = required;
		}

		public static void InitializeAttributes(object item)
		{
			Type envVarAttribute = typeof(EnvironmentVariableAttribute);
			foreach (var property in item.GetType().GetProperties())
			{
				if (GetCustomAttribute(property, envVarAttribute, false) is not EnvironmentVariableAttribute envVar)
					continue;
				var value = Environment.GetEnvironmentVariable(envVar.Name);
				if(value is null && envVar.Required)
					throw new ArgumentException($"Required environment variable {envVar.Name} is not set.", envVar.Name);
				property.SetValue(item, value);
			}
		}
	}
}