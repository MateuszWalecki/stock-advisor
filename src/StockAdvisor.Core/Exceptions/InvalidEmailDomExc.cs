using System;
using System.Net;

namespace StockAdvisor.Core.Exceptions
{
    public class InvalidEmailDomExc : DomainException
    {
        public InvalidEmailDomExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvalidEmailDomExc(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode => HttpStatusCode.BadRequest;

        public override string Code => "invalid_email";
    }
}