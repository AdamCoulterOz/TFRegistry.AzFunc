using System;
using System.Net;

namespace PurpleDepot.Interface.Exceptions
{
	public class NotFoundException : Exception
	{
		public static HttpStatusCode HttpStatusCode
			=> HttpStatusCode.NotFound;
		public string RequestType { get; }
		public NotFoundException(object obj) : base()
			=> RequestType = obj.GetType().Name;
		public override string Message
			=> $"{RequestType} doesn't exist";
	}
}
