using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class InvalidCredentialsSerExc : ServiceException
    {
        public InvalidCredentialsSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvalidCredentialsSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.Unauthorized;

        public override string Code =>
            "invalid_credentials";
    }
}