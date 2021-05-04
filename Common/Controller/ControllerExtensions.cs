using System;
using System.Net;
using System.Text.Json;
using System.Reflection;
using Microsoft.Azure.Functions.Worker.Http;

namespace PurpleDepot.Controller
{
	public static class ControllerExtensions
	{
		public static HttpResponseData CreateSerializedResponse(this HttpRequestData request, object document)
		{
			var body = JsonSerializer.Serialize(document);
			var response = request.CreateResponse(HttpStatusCode.OK);
			response.Headers.Add("Content-Type", "application/json");
			response.WriteString(body);
			return response;
		}

		public static void InitializeAttributes(this object item)
		{
			Type envVarAttribute = typeof(EnvironmentVariableAttribute);
			foreach (var property in item.GetType().GetProperties())
			{
				if (property.GetCustomAttribute(envVarAttribute, false) is not EnvironmentVariableAttribute envVar)
					continue;
				var value = Environment.GetEnvironmentVariable(envVar.Name);
				if(value is null && envVar.Required)
					throw new ArgumentException($"Required environment variable {envVar.Name} is not set.", envVar.Name);
				property.SetValue(item, value);
			}
		}
	}
}