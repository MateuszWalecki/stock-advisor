using System;
using System.Net;

namespace StockAdvisor.Core.Exceptions
{
    public class InvalidFirstNameDomExc : DomainException
    {
        public InvalidFirstNameDomExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvalidFirstNameDomExc(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode => HttpStatusCode.BadRequest;

        public override string Code => "invalid_first_name";
    }
}