using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);

            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    $"User with email {email} does not exist.");
            }

            return _mapper.Map<User, UserDto>(user);
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