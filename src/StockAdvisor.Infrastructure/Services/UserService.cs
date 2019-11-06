using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Extensions;

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
        
        public async Task<UserDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);

            return _mapper.Map<UserDto>(user);
        }
        
        public async Task<IEnumerable<UserDto>> BrowseAsync()
        {
            var users = await _userRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task ChangeUserPasswordAsync(Guid userId,
            string newPassword, string oldPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new InvalidPasswordSerExc("Password can not be empty.");
            }
            if (newPassword.Length < 4) 
            {
                throw new InvalidPasswordSerExc("Password must contain at least 4 characters.");
            }
            if (newPassword.Length > 100) 
            {
                throw new InvalidPasswordSerExc(
                    "Password can not contain more than 100 characters.");
            }

            var user = await _userRepository.GetUserOrFailAsync(userId);

            ValidateCurrentPassword(user, oldPassword);

            if (oldPassword == newPassword)
            {
                throw new InvalidPasswordSerExc("Old and new passwords cannot be the same");
            }

            var salt = _encrypter.GetSalt();
            var newPasswordHash = _encrypter.GetHash(newPassword, salt);

            user.SetPassword(newPasswordHash, salt);

            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangeUserEmailAsync(Guid userId, string password,
            string newEmail)
        {
            var user = await _userRepository.GetUserOrFailAsync(userId);

            ValidateCurrentPassword(user, password);

            ValidateEmail(newEmail);

            if (user.Email == newEmail)
            {
                throw new InvalidEmailSerExc($"Given email {newEmail} is already used.");
            }

            user.SetEmail(newEmail);

            await _userRepository.UpdateAsync(user);
        }

        public async Task LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);

            if (user == null)
            {
                throw new InvalidCredentialsSerExc("Invalid credentials.");
            }

            var hash = _encrypter.GetHash(password, user.Salt);

            if (hash != user.PasswordHash)
            {
                throw new InvalidCredentialsSerExc("Invalid credentials.");
            }
        }

        public async Task RegisterAsync(Guid userId, string email, string firstName,
            string surName, string password, UserRole userRole)
        {
            User existingUser = await _userRepository.GetAsync(email);

            if (existingUser != null)
            {
                throw new EmailInUseSerExc($"Given email {email} is in use.");
            }

            ValidateEmail(email);

            var salt = _encrypter.GetSalt();
            var hash = _encrypter.GetHash(password, salt);

            User newUser = new User(userId, email, firstName,
                surName, hash, salt, userRole);
            await _userRepository.AddAsync(newUser);
        }

        private void ValidateCurrentPassword(User user, string currentPassword)
        {
            var oldPasswordHash = _encrypter.GetHash(currentPassword, user.Salt);

            if (oldPasswordHash != user.PasswordHash)
            {
                throw new InvalidCredentialsSerExc("Given currently used password is invalid");
            }
        }

        private void ValidateEmail(string emailAddress)
        {
            try
            {
                var email = new MailAddress(emailAddress);
            }
            catch
            {
                throw new InvalidEmailSerExc($"Given email {emailAddress} is invalid.");
            }
        }
    }
}