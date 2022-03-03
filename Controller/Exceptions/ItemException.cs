using System.Net;
using PurpleDepot.Interface.Model;

namespace PurpleDepot.Controller.Exceptions;

public class ItemException<T> : HttpResponseException
	where T : RegistryItem<T>
{
	public Address<T> Address { get; }
	public ItemException(Address<T> address, HttpRequestMessage requestMessage, HttpStatusCode statusCode, string itemError)
		: base(requestMessage, statusCode, $"{typeof(T)} {itemError} at address {address}")
			=> Address = address;
}