using System.Collections.Generic;

namespace StockAdvisor.Core.Domain
{
    public class CompanyValueStatus
    {
        public Company Company { get; protected set; }
        public IEnumerable<CompanyValue> HistoricalValue { get; protected set; }

        public CompanyValueStatus(Company company, 
            IEnumerable<CompanyValue> historicalValue)
        {
            Company = company;
            HistoricalValue = historicalValue;
        }
        protected CompanyValueStatus()
        {
        }
    }
}