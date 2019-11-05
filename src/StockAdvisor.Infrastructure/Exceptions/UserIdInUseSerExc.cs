using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class UserIdInUseSerExc : ServiceException
    {
        public UserIdInUseSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public UserIdInUseSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.Conflict;

        public override string Code =>
            "user_id_is_in_use";
    }
}