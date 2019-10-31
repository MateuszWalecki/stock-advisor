using System.Collections.Generic;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Repositories.FakeDatabases
{
    public class FakeInvestorDatabase : IFakeInvestorDatabase
    {
        private readonly ISet<Investor> _investors = new HashSet<Investor>();

        public void Add(Investor investor)
            => _investors.Add(investor);

        public IEnumerable<Investor> GetAll()
            => _investors;

        public void Remove(Investor investor)
            => _investors.Remove(investor);
    }
}