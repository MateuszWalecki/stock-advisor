using System;
using System.Net;

namespace StockAdvisor.Core.Exceptions
{
    public class SymbolNotInSetDomExc : DomainException
    {
        public SymbolNotInSetDomExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public SymbolNotInSetDomExc(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode => HttpStatusCode.BadRequest;

        public override string Code => "company_symbol_not_in_set";
    }
}