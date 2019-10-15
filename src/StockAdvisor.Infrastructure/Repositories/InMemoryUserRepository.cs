using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static ISet<User> _users = new HashSet<User>
        {
            new User(Guid.NewGuid(), "first@example.com", "John", "Rambo", "Password1", "salt"),
            new User(Guid.NewGuid(), "second@example.com", "Sylvester", "Stalone", "Password2", "salt"),
            new User(Guid.NewGuid(), "third@example.com", "Johny", "Depp", "Password1", "salt"),
            new User(Guid.NewGuid(), "fourth@example.com", "John", "Travolta", "Password3", "salt")
        };

        public async Task AddAsync(User user)
        {
            _users.Add(user);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
            => await Task.FromResult(_users);

        public async Task<User> GetAsync(Guid id)
            => await Task.FromResult(_users.SingleOrDefault(x => x.Id == id));

        public async Task<User> GetAsync(string email)
            => await Task.FromResult(
                _users.SingleOrDefault(x => x.Email == email.ToLowerInvariant()));

        public async Task RemoveAsync(Guid id)
        {
            var userToDelte = await GetAsync(id); 

            if (userToDelte == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    "User not found.");
            }

            _users.Remove(userToDelte);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(User user)
		{
            // Current user is in app memory, so no need to update database etc.
			await Task.CompletedTask;
		}
    }
}