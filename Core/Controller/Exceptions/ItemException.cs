using System.Net;
using Interface.Model;

namespace Controller.Exceptions;

public class ItemException<T> : HttpResponseException
	where T : RegistryItem<T>
{
	protected ItemException(Address<T> address, HttpRequestMessage requestMessage, HttpStatusCode statusCode, string itemError)
		: base(requestMessage, statusCode, $"Terraform {typeof(T).Name} {itemError} at address: '{address}'")
	{
	}
}