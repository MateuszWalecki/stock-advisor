using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static readonly IEncrypter _encrypter = new Encrypter();
        private static readonly ISet<User> _users = new HashSet<User>();

        public async Task AddAsync(User user)
        {
            if (_users.Any(x => x.Email == user.Email))
            {
                throw new ServiceException(ErrorCodes.EmailInUse,
                    $"Email {user.Email} is in use.");
            }
            
            _users.Add(user);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> BrowseAsync()
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