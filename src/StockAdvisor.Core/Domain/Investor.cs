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
        public IEnumerable<string> FavouriteCompanies
        { 
            get {return _favouriteCompanies; }
            protected set
            {
                _favouriteCompanies = new HashSet<string>(value);
                UpdatedAt = DateTime.Now;
            }
        }

        private ISet<string> _favouriteCompanies = new HashSet<string>();

        protected Investor()
        {
        }

        public Investor(User user)
        {
            UserId = user.Id;
            UpdatedAt = DateTime.Now;
        }

        public void AddToFavouriteCompanies(string companySymbol)
        {
            var comapny = FavouriteCompanies.SingleOrDefault(x => x == companySymbol);

            if (comapny != null)
            {
                throw new SymbolInSetDomExc( $"Company represented by the symbol " +
                    $"'{companySymbol}' is currently included in the " +
                    $"favourite companies set, so it cannot be added.");
            }

            _favouriteCompanies.Add(companySymbol);
            UpdatedAt = DateTime.Now;
        }

        public void RemoveFromFavouriteCompanies(string companySymbol)
        {
            var comapny = FavouriteCompanies.SingleOrDefault(x => x == companySymbol);
            
            if (comapny == null)
            {
                throw new SymbolNotInSetDomExc($"Company represented by the symbol " +
                    $"'{companySymbol}' is not included in the " +
                    $"favourite companies set, so it cannot be removed.");
            }

            _favouriteCompanies.Remove(companySymbol);
            UpdatedAt = DateTime.Now;
        }
    }
}