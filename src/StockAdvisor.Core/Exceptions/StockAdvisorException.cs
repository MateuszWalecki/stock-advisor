using System;
using System.Net;

namespace StockAdvisor.Core.Exceptions
{
    public abstract class StockAdvisorException : Exception
    {
        public abstract HttpStatusCode CorrespondingStatusCode { get; }
        public abstract string Code { get; }

        protected StockAdvisorException()
        {
        }

        protected StockAdvisorException(string message, params object[] args)
            : this(null, message, args)
        {
        }

        protected StockAdvisorException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }
    }
}