using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    public abstract class InputDataBuilder : IInputDataBuilder
    {
        protected readonly IUserService UserService;
        protected readonly ILogger<InputDataBuilder> Logger;

        private int _createdInstanceId = 0;

        public InputDataBuilder(IUserService userService, ILogger<InputDataBuilder> logger)
        {
            UserService = userService;
            Logger = logger;
        }
        
        public async Task<UserWrapperForTesting> Build()
        {
            var newUser = CreateNewResource();
            await AddToRepos(newUser);
            Log(newUser);

            return newUser;
        }

        protected int GetNewInstaceId()
            => Interlocked.Increment(ref _createdInstanceId);

        protected abstract UserWrapperForTesting CreateNewResource();
        protected abstract Task AddToRepos(UserWrapperForTesting user);
        protected abstract void Log(UserWrapperForTesting user);
    }
}