using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class UserNotFoundSerExc : ServiceException
    {
        public UserNotFoundSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public UserNotFoundSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.NotFound;

        public override string Code =>
            "user_not_found";
    }
}