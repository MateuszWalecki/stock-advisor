using System;

namespace StockAdvisor.Core.Exceptions
{
    public abstract class DomainException : StockAdvisorException
    {
        protected DomainException(string message, params object[] args)
            : base(message, args)
        {
        }

        protected DomainException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }
    }
}