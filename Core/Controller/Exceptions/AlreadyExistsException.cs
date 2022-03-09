using System.Net;
using PurpleDepot.Core.Interface.Model;

namespace PurpleDepot.Core.Controller.Exceptions;
public class AlreadyExistsException<T> : ItemException<T>
	where T : RegistryItem<T>
{
	public AlreadyExistsException(Address<T> address, HttpRequestMessage requestMessage)
		: base(address, requestMessage, HttpStatusCode.Conflict, "already exists") { }
}
