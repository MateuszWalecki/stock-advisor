using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class InvalidCompanySymbolSerExc : ServiceException
    {
        public InvalidCompanySymbolSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public InvalidCompanySymbolSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.BadRequest;

        public override string Code =>
            "company_symbol_is_invalid";
    }
}