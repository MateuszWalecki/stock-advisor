using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class AdminBuilder : InputDataBuilder, IAdminBuilder
    {
        public AdminBuilder(IUserService userService, ILogger<InputDataBuilder> logger)
            : base(userService, logger)
        {
        }

        protected override UserWrapperForTesting CreateNewResource()
        {
            int i = GetNewInstaceId();

            return new UserWrapperForTesting(
                Guid.NewGuid(),
                $"admin{i}@admin.com",
                $"Leo{i}",
                $"Messi{i}",
                $"SuperSecret{i}",
                UserRole.Admin);
        }

        protected override async Task AddToRepos(UserWrapperForTesting newUser)
            => await UserService.RegisterAsync(
                newUser.Id, newUser.Email, newUser.FirstName, newUser.SurName, newUser.Password,
                newUser.Role);

        protected override void Log(UserWrapperForTesting newUser)
        {
            Logger.LogTrace($"Created admin for the email: {newUser.Email}.");
        }
    }
}