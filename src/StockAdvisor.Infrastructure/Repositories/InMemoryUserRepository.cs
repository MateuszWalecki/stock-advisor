using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Repositories.FakeDatabases;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository, InMemoryRepository
    {
        private readonly IEncrypter _encrypter = new Encrypter();
        private readonly IFakeUserDatabase _usersDB;

        public InMemoryUserRepository(IFakeUserDatabase fakeUsersDatabase)
        {
            _usersDB = fakeUsersDatabase;
        }

        public async Task AddAsync(User user)
        {
            if (_usersDB.GetAll().Any(x => x.Email == user.Email))
            {
                throw new EmailInUseSerExc( $"Email {user.Email} is in use.");
            }
            
            _usersDB.Add(user);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> BrowseAsync()
            => await Task.FromResult(_usersDB.GetAll());

        public async Task<User> GetAsync(Guid id)
            => await Task.FromResult(_usersDB.GetAll().SingleOrDefault(x => x.Id == id));

        public async Task<User> GetAsync(string email)
            => await Task.FromResult(
                _usersDB.GetAll().SingleOrDefault(x => x.Email == email?.ToLowerInvariant()));

        public async Task RemoveAsync(Guid id)
        {
            var userToDelte = await GetAsync(id); 

            if (userToDelte == null)
            {
                throw new UserNotFoundSerExc($"User with id {id} cannot be found.");
            }

            _usersDB.Remove(userToDelte);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(User user)
		{
            // Current user is in app memory, so no need to update database etc.
			await Task.CompletedTask;
		}
    }
}