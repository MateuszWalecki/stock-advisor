using System.Collections.Generic;

namespace StockAdvisor.Infrastructure.DTO
{
    public class CompanyValueStatusDto
    {
        public CompanyDto Company { get; set; }
        public IEnumerable<CompanyValueDto> HistoricalValue { get; set; }
    }
}