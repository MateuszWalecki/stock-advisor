using System;
using Newtonsoft.Json;

namespace StockAdvisor.Core.Domain
{
    public class CompanyValue
    {
        public DateTime Date { get; protected set; }
        [JsonProperty("close")]
        public decimal Price { get; protected set; }

        public CompanyValue(DateTime date, decimal price)
        {
            Date = date;
            Price = price;
        }
    }
}