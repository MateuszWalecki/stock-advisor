using System;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                SurName = user.SurName,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public async Task RegisterAsync(string email, string firstName,
            string surName, string password)
        {
            User existingUser = await _userRepository.GetAsync(email);

            if (existingUser != null)
            {
                throw new ServiceException(ErrorCodes.EmailInUse,
                    "Given email is in use.");
            }

            User newUser = new User(Guid.NewGuid(), email, firstName,
                surName, password, "salt");
            await _userRepository.AddAsync(newUser);
        }
    }
}