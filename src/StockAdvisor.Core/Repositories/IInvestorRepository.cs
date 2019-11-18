using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Core.Repositories
{
    public interface IInvestorRepository : IRepository
    {
        Task<Investor> GetAsync(Guid userId); 
        Task<IEnumerable<Investor>> BrowseAsync();
        Task AddAsync(Investor investor);
        Task UpdateAsync(Investor investor);
        Task RemoveAsync(Guid userId);
    }
}