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
        private readonly IEncrypter _encrypter;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IEncrypter encrypter,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _encrypter = encrypter;
            _mapper = mapper;
        }

        public async Task ChangeUserPasswordAsync(Guid userId,
            string newPassword, string oldPassword)
        {
            var user = await _userRepository.GetAsync(userId);
            
            user.ChangePassword(newPassword, oldPassword, "salt");
        }

        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);

            return _mapper.Map<User, UserDto>(user);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);

            if (user == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials,
                    "Invalid credentials.");
            }

            var hash = _encrypter.GetHash(password, user.Salt);

            if (hash != user.PasswordHash)
            {
                throw new ServiceException(ErrorCodes.InvalidCredentials,
                    "Invalid credentials.");
            }
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

            var salt = _encrypter.GetSalt(password);
            var hash = _encrypter.GetHash(password, salt);

            User newUser = new User(Guid.NewGuid(), email, firstName,
                surName, hash, salt);
            await _userRepository.AddAsync(newUser);
        }
    }
}