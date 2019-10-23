using System;
using Newtonsoft.Json;

namespace StockAdvisor.Core.Domain
{
    public class CompanyPrice
    {
        public DateTime Date { get; protected set; }
        [JsonProperty("close")]
        public decimal Price { get; protected set; }

        public CompanyPrice(DateTime date, decimal price)
        {
            Date = date;
            Price = price;
        }
    }
}