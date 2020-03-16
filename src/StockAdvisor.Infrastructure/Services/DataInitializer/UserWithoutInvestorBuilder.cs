using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class UserWithoutInvestorBuilder : InputDataBuilder, IUserWithoutInvestorBuilder
    {
        public UserWithoutInvestorBuilder(IUserService userService, ILogger<InputDataBuilder> logger)
            : base(userService, logger)
        {
        }

        protected override UserWrapperForTesting CreateNewResource()
        {
            int i = GetNewInstaceId();

            return new UserWrapperForTesting(
                Guid.NewGuid(),
                $"user_without_investor{i}@test.com",
                $"Sylvester{i}",
                $"Stalone{i}",
                $"Secret{i}",
                UserRole.User);
        }

        protected override async Task AddToRepos(UserWrapperForTesting newUser)
            => await UserService.RegisterAsync(
                newUser.Id, newUser.Email, newUser.FirstName, newUser.SurName, newUser.Password,
                newUser.Role);

        protected override void Log(UserWrapperForTesting newUser)
        {
            Logger.LogTrace($"Created user without investor for the email: {newUser.Email}.");
        }
    }
}