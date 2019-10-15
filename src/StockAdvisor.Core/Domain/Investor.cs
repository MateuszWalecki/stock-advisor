using System;
using System.Collections.Generic;

namespace StockAdvisor.Core.Domain
{
    public class Investor
    {
        public Guid UserId { get; protected set; }
        public IEnumerable<Company> FavouriteCompanies { get; protected set; }

        protected Investor()
        {
        }

        public Investor(Guid userId)
        {
            UserId = userId;
        }
    }
}