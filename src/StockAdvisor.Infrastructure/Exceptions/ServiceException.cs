using System;
using System.Net;
using StockAdvisor.Core.Exceptions;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public abstract class ServiceException : StockAdvisorException
    {
        protected ServiceException(string message, params object[] args)
            : base(message, args)
        {
        }

        protected ServiceException(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }
    }
}