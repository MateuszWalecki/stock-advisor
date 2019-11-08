using System;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public class AdminBuilder : InputDataBuilder
    {
        public AdminBuilder(IUserService userService, ILogger<InputDataBuilder> logger) : base(userService, logger)
        {
        }

        protected override void CreateNewResource()
        {
            int i = CreatedInstanceId;
            CreatedInstanceId++;

            dynamic newAdmin = new ExpandoObject();

            newAdmin.Id = Guid.NewGuid();
            newAdmin.Email = $"admin{i}@admin.com";
            newAdmin.FirstName = $"Leo{i}";;
            newAdmin.SurName = $"Messi{i}";
            newAdmin.Role =  UserRole.Admin;
            newAdmin.Password = $"SuperSecret{i}";

            NewResourceToAdd = newAdmin;
        }

        protected override async Task AddToRepos()
        {
            dynamic newAdmin = NewResourceToAdd;

            await UserService.RegisterAsync(newAdmin.Id, newAdmin.Email, newAdmin.FirstName,
                newAdmin.SurName, newAdmin.Password, newAdmin.Role);
        }

        protected override void Log()
        {
            Logger.LogTrace($"Created admin for the email: {NewResourceToAdd.Email}.");
        }
    }
}