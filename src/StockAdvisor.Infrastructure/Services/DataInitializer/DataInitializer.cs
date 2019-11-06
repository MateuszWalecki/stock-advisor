using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Services.TasksHandling;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        private readonly UserWithoutInvestorBuilder _userWithoutInvestorBuilder;
        private readonly UserWithInvestorBuilder _userWithInvestorBuilder;
        private readonly AdminBuilder _adminBuilder;

        private readonly ILogger<DataInitializer> _logger;

        public DataInitializer(UserWithoutInvestorBuilder userWithoutInvestorBuilder,
            UserWithInvestorBuilder userWithInvestorBuilder, AdminBuilder adminBuilder,
            ILogger<DataInitializer> logger)
        {
            _userWithoutInvestorBuilder = userWithoutInvestorBuilder;
            _userWithInvestorBuilder = userWithInvestorBuilder;
            _adminBuilder = adminBuilder;

            _logger = logger;
        }

        public async Task<ExpandoObject> AddAndGetNextUserWithInvestor()
        {
            var newUserWithInvestor = await _userWithInvestorBuilder.Build();

            return newUserWithInvestor;
        }

        public async Task<ExpandoObject> AddAndGetNextUserWithoutInvestor()
        {
            var newUserWithoutInvestor = await _userWithoutInvestorBuilder.Build();

            return newUserWithoutInvestor;
        }

        public async Task SeedDefaultAsync()
        {
            _logger.LogTrace("Initializing data...");

            var tasks = new List<Task>();
            
            for (int i = 1; i <= 10; i++)
            {
                tasks.Add(_userWithInvestorBuilder.Build());
            }

            for (int i = 1; i <= 10; i++)
            {
                tasks.Add(_userWithoutInvestorBuilder.Build());
            }

            for (int i = 1; i <= 3; i++)
            {
                tasks.Add(_adminBuilder.Build());
            }

            await Task.WhenAll(tasks);

            _logger.LogTrace("Data was initialized.");
        }
    }
}