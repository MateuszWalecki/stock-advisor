using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly IInvestorService _investorService;
        private readonly ILogger<DataInitializer> _logger;
        private readonly IEncrypter _encrypter;
        private int _nextUserWithInvestorIdx = 0;
        private int _nextUserWithoutInvestorIdx = 0;

        public DataInitializer(IUserService userService, IInvestorService investorService,
            ILogger<DataInitializer> logger, IEncrypter encrypter)
        {
            _userService = userService;
            _investorService = investorService;
            _logger = logger;
            _encrypter = encrypter;
        }

        public async Task<ExpandoObject> AddAndGetNextUserWithInvestor()
        {
            int i = _nextUserWithInvestorIdx;
            _nextUserWithInvestorIdx++;

            dynamic newUser = new ExpandoObject();
            newUser.Id = Guid.NewGuid();
            newUser.Email = $"user_with_investor{i}@test.com";
            newUser.FirstName = $"John{i}";;
            newUser.SurName = $"Rambo{i}";
            newUser.Role =  UserRole.User.ToString();
            newUser.Password = $"Secret{i}";
            
            await _userService.RegisterAsync(newUser.Id, newUser.Email, newUser.FirstName,
                newUser.SurName, newUser.Password, UserRole.User);
            await _investorService.RegisterAsync(newUser.Id);

            _logger.LogTrace($"Created user and investor for the email: {newUser.Email}.");

            return newUser;
        }

        public async Task<ExpandoObject> AddAndGetNextUserWithoutInvestor()
        {
            int i = _nextUserWithoutInvestorIdx;
            _nextUserWithoutInvestorIdx++;

            dynamic newUser = new ExpandoObject();
            newUser.Id = Guid.NewGuid();
            newUser.Email = $"user_without_investor{i}@test.com";
            newUser.FirstName = $"Sylvester{i}";;
            newUser.SurName = $"Stalone{i}";
            newUser.Role =  UserRole.User.ToString();
            newUser.Password = $"Secret{i}";
            
            await _userService.RegisterAsync(newUser.Id, newUser.Email, newUser.FirstName,
                newUser.SurName, newUser.Password, UserRole.User);

            _logger.LogTrace($"Created user without investor for the email: {newUser.Email}.");

            return newUser;
        }

        public async Task SeedAsync()
        {
            _logger.LogTrace("Initializing data...");

            var tasks = new List<Task>();
            
            for (int i = 1; i <= 10; i++)
            {
                var userId = Guid.NewGuid();
                var email = $"user{i}@default.com";
                
                tasks.Add(_userService.RegisterAsync(userId, email, 
                    $"Firstname{i}", $"Surname{i}", $"Secret{i}", UserRole.User));
                tasks.Add(_investorService.RegisterAsync(userId));

                _logger.LogTrace($"Created user and investor for the email: {email}.");
            }

            for (int i = 1; i <= 5; i++)
            {
                var userId = Guid.NewGuid();
                var email = $"without_investor{i}@default.com";
                
                tasks.Add(_userService.RegisterAsync(userId, email, 
                    $"Firstname{i}", $"Surname{i}", $"Secret{i}", UserRole.User));

                _logger.LogTrace($"Created user for the email: {email}.");
            }

            for (int i = 1; i <= 3; i++)
            {
                var adminId = Guid.NewGuid();
                var adminName = $"admin{i}";
                var email = $"admin{i}@test.com";
                tasks.Add(_userService.RegisterAsync(adminId, email, 
                    $"Firstname{i}", $"Surname{i}", $"Secret{i}", UserRole.Admin));
            }

            await Task.WhenAll(tasks);

            _logger.LogTrace("Data was initialized.");
        }
    }
}