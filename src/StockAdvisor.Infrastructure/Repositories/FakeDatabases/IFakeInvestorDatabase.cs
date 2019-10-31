using System.Collections.Generic;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Repositories.FakeDatabases
{
    public interface IFakeInvestorDatabase : IFakeDatabase
    {
        void Add(Investor investor);
        IEnumerable<Investor> GetAll();
        void Remove(Investor investor);
    }
}