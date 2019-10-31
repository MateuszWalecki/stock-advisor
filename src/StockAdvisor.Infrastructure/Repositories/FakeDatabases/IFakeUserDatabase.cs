using System.Collections.Generic;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Repositories.FakeDatabases
{
    public interface IFakeUserDatabase : IFakeDatabase
    {
        void Add(User user);
        IEnumerable<User> GetAll();
        void Remove(User user);
    }
}