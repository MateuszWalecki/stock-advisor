using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Services.TasksHandling;

namespace StockAdvisor.Infrastructure.Handlers.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand>
    {
        private readonly IHandler _handler;
        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;
        
        public LoginHandler(IHandler handler, IUserService userService,
            IJwtHandler jwtHandler, IMemoryCache cache)
        {
            _handler = handler;
            _userService = userService;
            _jwtHandler = jwtHandler;
            _cache = cache;
        }

        public async Task HandleAsync(LoginCommand command)
            => await _handler
                .Run(async () => await _userService.LoginAsync(command.Email, command.Password))
                .OnSuccess(async () => 
                {
                    var user = await _userService.GetAsync(command.Email);
                    var jwt = _jwtHandler.CreateToken(user.Id, user.Role);
                    _cache.SetJwt(command.TokenId, jwt);
                })
                .Next()
                .ExecuteAllAsync();
    }
}