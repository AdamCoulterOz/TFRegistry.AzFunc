using System.Net;
using PurpleDepot.Core.Interface.Model;

namespace PurpleDepot.Core.Controller.Exceptions;

public class ItemException<T> : ControllerResultException
	where T : RegistryItem<T>
{
	protected ItemException(Address<T> address, HttpStatusCode statusCode, string itemError)
		: base(statusCode, $"Terraform {typeof(T).Name} {itemError} at address: '{address}'") { }
}