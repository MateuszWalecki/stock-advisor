using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class CommandNullSerExc : ServiceException
    {
        public CommandNullSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public CommandNullSerExc(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode => HttpStatusCode.BadRequest;

        public override string Code => "null_command";
    }
}