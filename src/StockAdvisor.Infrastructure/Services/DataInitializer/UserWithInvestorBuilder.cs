using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class UserWithInvestorBuilder : InputDataBuilder, IUserWithInvestorBuilder
    {
        private readonly IInvestorService _investorService;

        public UserWithInvestorBuilder(IUserService userService, ILogger<InputDataBuilder> logger,
            IInvestorService investorService)
            : base(userService, logger)
        {
            _investorService = investorService;
        }

        protected override UserWrapperForTesting CreateNewResource()
        {
            int i = GetNewInstaceId();

            return new UserWrapperForTesting(
                Guid.NewGuid(),
                $"user_with_investor{i}@test.com",
                $"John{i}",
                $"Rambo{i}",
                $"Secret{i}",
                UserRole.User);
        }

        protected override async Task AddToRepos(UserWrapperForTesting newUser)
        {
            await UserService.RegisterAsync(newUser.Id, newUser.Email, newUser.FirstName,
                newUser.SurName, newUser.Password, newUser.Role);
            
            await _investorService.RegisterAsync(newUser.Id);

            await _investorService.AddToFavouriteCompaniesAsync(newUser.Id, "AAPL");
            await _investorService.AddToFavouriteCompaniesAsync(newUser.Id, "BAC");
        }

        protected override void Log(UserWrapperForTesting newUser)
        {
            Logger.LogTrace($"Created user and investor for the email: {newUser.Email}.");
        }
    }
}