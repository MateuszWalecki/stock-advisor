using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        private readonly UserWithoutInvestorBuilder _userWithoutInvestorBuilder;
        private readonly UserWithInvestorBuilder _userWithInvestorBuilder;
        private readonly AdminBuilder _adminBuilder;

        private readonly IUserService _userService;
        private readonly ILogger<DataInitializer> _logger;

        public DataInitializer(UserWithoutInvestorBuilder userWithoutInvestorBuilder,
            UserWithInvestorBuilder userWithInvestorBuilder, AdminBuilder adminBuilder,
            IUserService userService, ILogger<DataInitializer> logger)
        {
            _userWithoutInvestorBuilder = userWithoutInvestorBuilder;
            _userWithInvestorBuilder = userWithInvestorBuilder;
            _adminBuilder = adminBuilder;

            _userService = userService;
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
            if(await UsersHasBeenInitialized())
            {
                return;
            }

            _logger.LogTrace("Initializing data...");

            var tasks = new List<Task>();
            
            for (int i = 1; i <= 10; i++)
            {
                await _userWithInvestorBuilder.Build();
            }

            for (int i = 1; i <= 10; i++)
            {
                await _userWithoutInvestorBuilder.Build();
            }

            for (int i = 1; i <= 3; i++)
            {
                await _adminBuilder.Build();
            }

            await Task.WhenAll(tasks);

            _logger.LogTrace("Data was initialized.");
        }

        private async Task<bool> UsersHasBeenInitialized()
        {
            var users = await _userService.BrowseAsync();

            return users.Any();
        }
    }
}