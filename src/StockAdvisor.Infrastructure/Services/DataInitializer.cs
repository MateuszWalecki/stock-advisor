using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly IInvestorService _investorService;
        private readonly ILogger<DataInitializer> _logger;

        public DataInitializer(IUserService userService, IInvestorService investorService,
            ILogger<DataInitializer> logger)
        {
            _userService = userService;
            _investorService = investorService;
            _logger = logger;
        }
        
        public async Task SeedAsync()
        {
            _logger.LogTrace("Initializing data...");

            var tasks = new List<Task>();
            
            for (int i = 1; i <= 10; i++)
            {
                var userId = Guid.NewGuid();
                var email = $"user{i}@test.com";
                
                tasks.Add(_userService.RegisterAsync(userId, email, 
                    $"Firstname{i}", $"Surname{i}", $"Secret{i}", UserRole.User));
                tasks.Add(_investorService.RegisterAsync(userId));

                _logger.LogTrace($"Created user and investor for the email: {email}.");
            }

            for (int i = 1; i <= 5; i++)
            {
                var userId = Guid.NewGuid();
                var email = $"without_investor{i}@test.com";
                
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