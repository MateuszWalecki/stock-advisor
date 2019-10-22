using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Core.Repositories;

namespace StockAdvisor.Core.Domain
{
    public class Company
    {
        public string Symbol { get; protected set; }
        public string Name { get; protected set; }
        public decimal Price { get; protected set; }

        public Company(string symbol, string name, decimal price)
        {
            Symbol = symbol;
            Name = name;
            Price = price;
        }
    }
}