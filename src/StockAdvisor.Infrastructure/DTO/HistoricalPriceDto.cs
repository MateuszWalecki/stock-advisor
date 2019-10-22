using System;

namespace StockAdvisor.Infrastructure.DTO
{
    public class HistoricalPriceDto
    {
        public DateTime Date { get; set; }
        decimal Price { get; set; }
    }
}