using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class CompanySymbolInUseSerExc : ServiceException
    {
        public CompanySymbolInUseSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public CompanySymbolInUseSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.Conflict;

        public override string Code =>
            "company_symbol_in_use";
    }
}