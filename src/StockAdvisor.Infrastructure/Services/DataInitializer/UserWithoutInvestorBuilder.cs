using System;
using System.Dynamic;
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

        protected override void CreateNewResource()
        {
            int i = CreatedInstanceId;
            CreatedInstanceId++;

            dynamic newUser = new ExpandoObject();

            newUser.Id = Guid.NewGuid();
            newUser.Email = $"user_without_investor{i}@test.com";
            newUser.FirstName = $"Sylvester{i}";;
            newUser.SurName = $"Stalone{i}";
            newUser.Role =  UserRole.User;
            newUser.Password = $"Secret{i}";

            NewResourceToAdd = newUser;
        }

        protected override async Task AddToRepos()
        {
            dynamic newUser = NewResourceToAdd;

            await UserService.RegisterAsync(newUser.Id, newUser.Email, newUser.FirstName,
                newUser.SurName, newUser.Password, newUser.Role);
        }

        protected override void Log()
        {
            Logger.LogTrace($"Created user without investor for the email: {NewResourceToAdd.Email}.");
        }
    }
}