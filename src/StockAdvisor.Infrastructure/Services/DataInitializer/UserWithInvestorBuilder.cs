using System;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class UserWithInvestorBuilder : InputDataBuilder
    {
        private readonly IInvestorService _investorService;

        public UserWithInvestorBuilder(IUserService userService, ILogger<InputDataBuilder> logger,
            IInvestorService investorService)
            : base(userService, logger)
        {
            _investorService = investorService;
        }

        protected override void CreateNewResource()
        {
            int i = CreatedInstanceId;
            CreatedInstanceId++;

            dynamic newUser = new ExpandoObject();

            newUser.Id = Guid.NewGuid();
            newUser.Email = $"user_with_investor{i}@test.com";
            newUser.FirstName = $"John{i}";;
            newUser.SurName = $"Rambo{i}";
            newUser.Role =  UserRole.User;
            newUser.Password = $"Secret{i}";

            NewResourceToAdd = newUser;
        }

        protected override async Task AddToRepos()
        {
            dynamic newUser = NewResourceToAdd;

            await UserService.RegisterAsync(newUser.Id, newUser.Email, newUser.FirstName,
                newUser.SurName, newUser.Password, newUser.Role);
            
            await _investorService.RegisterAsync(newUser.Id);

            await _investorService.AddToFavouriteCompaniesAsync(newUser.Id, "AAPL");
        }

        protected override void Log()
        {
            Logger.LogTrace($"Created user and investor for the email: {NewResourceToAdd.Email}.");
        }
    }
}