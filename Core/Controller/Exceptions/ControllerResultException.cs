using System.Net;

namespace PurpleDepot.Core.Controller.Exceptions;
public class ControllerResultException : Exception
{
	public ControllerResult Response { get; }

	public ControllerResultException(HttpStatusCode statusCode, string message = "")
		: base(message) => Response = ControllerResult.New(statusCode, message);
}
