using System;

namespace StockAdvisor.Infrastructure.DTO
{
    public class CompanyDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}