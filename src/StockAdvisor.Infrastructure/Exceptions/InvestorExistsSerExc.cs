using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class InvestorExistsSerExc : ServiceException
    {
        public InvestorExistsSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvestorExistsSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.Conflict;

        public override string Code =>
            "investor_exists";
    }
}