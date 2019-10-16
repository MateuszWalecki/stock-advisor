using System;
using System.Collections.Generic;
using System.Linq;
using StockAdvisor.Core.Exceptions;

namespace StockAdvisor.Core.Domain
{
    public class Investor
    {
        public Guid UserId { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public IEnumerable<string> FavouriteCompanies { 
            get {return _favouriteCompanies; } }

        private ISet<string> _favouriteCompanies = new HashSet<string>();

        protected Investor()
        {
        }

        public Investor(Guid userId)
        {
            UserId = userId;
        }

        public void AddToFavouriteCompanies(string companySymbol)
        {
            var comapny = FavouriteCompanies.SingleOrDefault(x => x == companySymbol);

            if (comapny != null)
            {
                throw new DomainException(ErrorCodes.ElementInSet,
                    $"Company represented by the set {companySymbol} is currently included in the " +
                    $"FavouriteCompanies set, so it cannot be added.");
            }

            _favouriteCompanies.Add(companySymbol);
            UpdatedAt = DateTime.Now;
        }

        public void RemoveFromFavouriteCompanies(string companySymbol)
        {
            var comapny = FavouriteCompanies.SingleOrDefault(x => x == companySymbol);
            
            if (comapny == null)
            {
                throw new DomainException(ErrorCodes.ElementNotInSet,
                    $"Company represented by the set {companySymbol} is not included in the " +
                    $"FavouriteCompanies set, so it cannot be removed.");
            }

            _favouriteCompanies.Remove(companySymbol);
            UpdatedAt = DateTime.Now;
        }
    }
}