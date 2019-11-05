using System;

namespace StockAdvisor.Core.Exceptions
{
    public class DomainException : StockAdvisorException
    {
        private readonly string _code;

        public DomainException()
        {
        }

        public DomainException(string message, params object[] args) : base(string.Empty, message, args)
        {
        }

        public DomainException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public override string Code => _code;
    }
}