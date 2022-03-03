using System.Net;
using Interface.Model;

namespace Controller.Exceptions;
public class AlreadyExistsException<T> : ItemException<T>
	where T : RegistryItem<T>
{
	public AlreadyExistsException(Address<T> address, HttpRequestMessage requestMessage)
		: base(address, requestMessage, HttpStatusCode.Conflict, "already exists") { }
}
