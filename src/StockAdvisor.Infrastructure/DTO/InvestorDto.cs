using System;
using System.Collections.Generic;

namespace StockAdvisor.Infrastructure.DTO
{
    public class InvestorDto
    {
        public Guid UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<string> FavouriteCompanies { get; set; }
    }
}