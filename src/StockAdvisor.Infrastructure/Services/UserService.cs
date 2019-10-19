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
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, 
                    "Password can not be empty.");
            }
            if (newPassword.Length < 4) 
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, 
                    "Password must contain at least 4 characters.");
            }
            if (newPassword.Length > 100) 
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, 
                    "Password can not contain more than 100 characters.");
            }

            var user = await _userRepository.GetAsync(userId);

            var oldPasswordHash = _encrypter.GetHash(oldPassword, user.Salt);

            if (oldPasswordHash != user.PasswordHash)
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, 
                    "Given currently used password is invalid");
            }

            if (oldPassword == newPassword)
            {
                throw new ServiceException(ErrorCodes.InvalidPassword, 
                    "Old and new passwords cannot be the same");
            }

            var salt = _encrypter.GetSalt();
            var newPasswordHash = _encrypter.GetHash(newPassword, salt);

            user.SetPassword(newPasswordHash, salt);
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

            var salt = _encrypter.GetSalt();
            var hash = _encrypter.GetHash(password, salt);

            User newUser = new User(Guid.NewGuid(), email, firstName,
                surName, hash, salt);
            await _userRepository.AddAsync(newUser);
        }
    }
}