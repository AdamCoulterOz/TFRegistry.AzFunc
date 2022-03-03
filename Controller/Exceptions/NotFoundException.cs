using System.Net;
using Interface.Model;

namespace Controller.Exceptions;
public class NotFoundException<T> : ItemException<T>
	where T : RegistryItem<T>
{
	public NotFoundException(Address<T> address, HttpRequestMessage requestMessage)
		: base(address, requestMessage, HttpStatusCode.NotFound, "doesn't exist") { }
}
