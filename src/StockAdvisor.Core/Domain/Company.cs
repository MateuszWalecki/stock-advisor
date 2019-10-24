using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockAdvisor.Core.Repositories;

namespace StockAdvisor.Core.Domain
{
    public class Company
    {
        public string Symbol { get; protected set; }
        public string Name { get; protected set; }
        [JsonProperty("price")]
        public decimal CurrentPrice { get; protected set; }
        
        public Company(string symbol, string name, decimal currentPrice)
        {
            Symbol = symbol;
            Name = name;
            CurrentPrice = currentPrice;
        }
    }
}