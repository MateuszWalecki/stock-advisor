using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class EmailInUseSerExc : ServiceException
    {
        public EmailInUseSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public EmailInUseSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.Conflict;

        public override string Code =>
            "email_in_use";
    }
}