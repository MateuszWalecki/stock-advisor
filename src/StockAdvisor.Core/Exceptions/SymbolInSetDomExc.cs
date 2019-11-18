using System;
using System.Net;

namespace StockAdvisor.Core.Exceptions
{
    public class SymbolInSetDomExc : DomainException
    {
        public SymbolInSetDomExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public SymbolInSetDomExc(Exception innerException, string message, params object[] args)
            : base(innerException, message, args)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode => HttpStatusCode.Conflict;

        public override string Code => "company_symbol_in_set";
    }
}