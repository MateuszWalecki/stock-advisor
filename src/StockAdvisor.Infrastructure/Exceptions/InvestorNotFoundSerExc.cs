using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class InvestorNotFoundSerExc : ServiceException
    {
        public InvestorNotFoundSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvestorNotFoundSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.NotFound;

        public override string Code =>
            "investor_not_found";
    }
}