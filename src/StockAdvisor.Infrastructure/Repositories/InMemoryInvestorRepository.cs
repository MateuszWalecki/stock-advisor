using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class InMemoryInvestorRepository : IInvestorRepository
    {
        private static readonly ISet<Investor> _investors = new HashSet<Investor>()
        {
            new Investor(Guid.NewGuid()),
            new Investor(Guid.NewGuid()),
            new Investor(Guid.NewGuid()),
            new Investor(Guid.NewGuid())
        };

        public async Task AddAsync(Investor investor)
        {
            _investors.Add(investor);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Investor>> GetAllAsync()
            => await Task.FromResult(_investors);

        public async Task<Investor> GetAsync(Guid userId)
            => await Task.FromResult(_investors.SingleOrDefault(x => x.UserId == userId));

        public async Task RemoveAsync(Investor investor)
        {
            if (!_investors.Contains(investor))
            {
                throw new ServiceException(ErrorCodes.InvestorNotFound,
                    "Investor cannot be removed from the repository, because it does not exists there.");
            }

            _investors.Remove(investor);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Investor investor)
        {
            // Current investor is in app memory, so no need to update database etc.
            await Task.CompletedTask;
        }
    }
}