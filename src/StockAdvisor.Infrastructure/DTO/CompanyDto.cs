using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StockAdvisor.Infrastructure.DTO
{
    public class CompanyDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal CurrentPrice { get; set; }
        public IEnumerable<HistoricalPriceDto> HistoricalPrice { get; set; }
    }
}