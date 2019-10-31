using System.Collections.Generic;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Repositories.FakeDatabases
{
    public class FakeUserDatabase : IFakeUserDatabase
    {
        private readonly ISet<User> _users  = new HashSet<User>();

        public void Add(User user)
            => _users.Add(user);

        public IEnumerable<User> GetAll()
            => _users;

        public void Remove(User user)
            => _users.Remove(user);
    }
}