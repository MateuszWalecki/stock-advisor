using System;

namespace StockAdvisor.Core.Exceptions
{
    public class DomainException : StockAdvisorException
    {
        private readonly string _code;

        public DomainException()
        {
        }

        public DomainException(string code) : base(code)
        {
            _code = code;
        }

        public DomainException(string code, string message, params object[] args)
            : base(null, message, args)
        {
            _code = code;
        }

        public DomainException(Exception innerException, string code, string message, params object[] args)
            : base(innerException, message, args)
        {
            _code = code;
        }        

        public override string Code => _code;
    }
}