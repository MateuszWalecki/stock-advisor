using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Core.Repositories
{
    public interface ICompanyRepository : IRepository
    {
        Task<Company> GetAsync(string companySymbol); 
        Task<IEnumerable<Company>> BrowseAsync();
    }
}