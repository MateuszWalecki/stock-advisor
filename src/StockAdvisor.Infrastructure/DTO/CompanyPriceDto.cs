using System;

namespace StockAdvisor.Infrastructure.DTO
{
    public class CompanyPriceDto
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}