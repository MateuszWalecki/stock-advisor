using System;

namespace StockAdvisor.Core.Domain
{
    public class StockValue
    {
        public DateTime Date { get; protected set; }
        public decimal Price { get; protected set; }
    }
}