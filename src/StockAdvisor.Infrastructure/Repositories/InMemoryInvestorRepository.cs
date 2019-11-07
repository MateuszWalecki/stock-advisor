using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Repositories.FakeDatabases;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class InMemoryInvestorRepository : IInvestorRepository, InMemoryRepository
    {
        private readonly IFakeInvestorDatabase _investorsDB;

        public InMemoryInvestorRepository(IFakeInvestorDatabase fakeDB)
        {
            _investorsDB = fakeDB;
        }

        public async Task AddAsync(Investor investor)
        {
            if (_investorsDB.GetAll().Any(x => x.UserId == investor.UserId))
            {
                throw new UserIdInUseSerExc($"User with id {investor.UserId} is in use.");
            }

            _investorsDB.Add(investor);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Investor>> BrowseAsync()
            => await Task.FromResult(_investorsDB.GetAll());

        public async Task<Investor> GetAsync(Guid userId)
            => await Task.FromResult(_investorsDB.GetAll().SingleOrDefault(x => x.UserId == userId));

        public async Task RemoveAsync(Investor investor)
        {
            if (!_investorsDB.GetAll().Contains(investor))
            {
                throw new InvestorNotFoundSerExc($"Investor cannot be removed from the repository, " +
                    "because it does not exists there.");
            }

            _investorsDB.Remove(investor);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Investor investor)
        {
            // Current investor is in app memory, so no need to update database etc.
            await Task.CompletedTask;
        }
    }
}