using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class CompanySymbolNotFoundSerExc : ServiceException
    {
        public CompanySymbolNotFoundSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public CompanySymbolNotFoundSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.NotFound;

        public override string Code =>
            "company_symbol_not_found";
    }
}