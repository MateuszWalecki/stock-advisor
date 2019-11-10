using System;
using System.Net;

namespace StockAdvisor.Infrastructure.Exceptions
{
    public class WrongAlgorithmNameSerExc : ServiceException
    {
        public WrongAlgorithmNameSerExc(string message, params object[] args)
            : this(null, message, args)
        {
        }

        public WrongAlgorithmNameSerExc(Exception innerException, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }

        public override HttpStatusCode CorrespondingStatusCode =>
            HttpStatusCode.BadRequest;

        public override string Code =>
            "wrong_algorithm_name";
    }
}