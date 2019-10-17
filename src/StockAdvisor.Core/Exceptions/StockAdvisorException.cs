using System;

namespace StockAdvisor.Core.Exceptions
{
    public abstract class StockAdvisorException : Exception
    {
        public string Code { get; }

        protected StockAdvisorException()
        {
        }

        protected StockAdvisorException(string code)
        {
            Code = code;
        }

        protected StockAdvisorException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        protected StockAdvisorException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        protected StockAdvisorException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        protected StockAdvisorException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}