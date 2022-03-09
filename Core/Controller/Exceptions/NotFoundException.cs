using System.Net;
using PurpleDepot.Core.Interface.Model;

namespace PurpleDepot.Core.Controller.Exceptions;
public class NotFoundException<T> : ItemException<T>
	where T : RegistryItem<T>
{
	public NotFoundException(Address<T> address, HttpRequestMessage requestMessage)
		: base(address, requestMessage, HttpStatusCode.NotFound, "doesn't exist") { }
}
