using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class InvalidEmailSerExc : ServiceException
    {
        public InvalidEmailSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvalidEmailSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.BadRequest;

        public override string Code =>
            "invalid_email";
    }
}