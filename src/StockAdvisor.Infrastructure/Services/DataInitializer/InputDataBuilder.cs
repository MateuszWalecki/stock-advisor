using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public abstract class InputDataBuilder : IInputDataBuilder
    {
        protected readonly IUserService UserService;
        protected readonly ILogger<InputDataBuilder> Logger;
        protected int CreatedInstanceId = 0;
        protected dynamic NewResourceToAdd;

        public InputDataBuilder(IUserService userService, ILogger<InputDataBuilder> logger)
        {
            UserService = userService;
            Logger = logger;
        }
        
        public async Task<dynamic> Build()
        {
            CreateNewResource();
            await AddToRepos();
            Log();

            return NewResourceToAdd;
        }

        protected abstract void CreateNewResource();
        protected abstract Task AddToRepos();
        protected abstract void Log();
    }
}