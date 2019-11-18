using System;
using System.Net;

namespace StockAdvisor.Core.Exceptions
{
    public class InvalidSurNameDomExc : DomainException
    {
        public InvalidSurNameDomExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvalidSurNameDomExc(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode => HttpStatusCode.BadRequest;

        public override string Code => "invalid_surname";
    }
}