using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserWithoutInvestorBuilder _userWithoutInvestorBuilder;
        private readonly IUserWithInvestorBuilder _userWithInvestorBuilder;
        private readonly IAdminBuilder _adminBuilder;

        private readonly IUserService _userService;
        private readonly ILogger<DataInitializer> _logger;

        public DataInitializer(IUserWithoutInvestorBuilder userWithoutInvestorBuilder,
            IUserWithInvestorBuilder userWithInvestorBuilder, IAdminBuilder adminBuilder,
            IUserService userService, ILogger<DataInitializer> logger)
        {
            _userWithoutInvestorBuilder = userWithoutInvestorBuilder;
            _userWithInvestorBuilder = userWithInvestorBuilder;
            _adminBuilder = adminBuilder;

            _userService = userService;
            _logger = logger;
        }

        public async Task<UserWrapperForTesting> AddAndGetNextUserWithInvestor()
            => await _userWithInvestorBuilder.Build();

        public async Task<UserWrapperForTesting> AddAndGetNextUserWithoutInvestor()
            => await _userWithoutInvestorBuilder.Build();

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