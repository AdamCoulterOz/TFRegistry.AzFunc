using System.Net;
using PurpleDepot.Core.Interface.Model;

namespace PurpleDepot.Core.Controller.Exceptions;
public class AlreadyExistsException<T> : ItemException<T>
	where T : RegistryItem<T>
{
	public AlreadyExistsException(Address<T> address)
		: base(address, HttpStatusCode.Conflict, "already exists") { }
}
