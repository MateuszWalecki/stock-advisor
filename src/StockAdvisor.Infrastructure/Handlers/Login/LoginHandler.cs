using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand>
    {
        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMemoryCache _cache;
        
        public LoginHandler(IUserService userService, IJwtHandler jwtHandler,
            IMemoryCache cache)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
            _cache = cache;
        }

        public async Task HandleAsync(LoginCommand command)
        {
            await _userService.LoginAsync(command.Email, command.Password);

            var user = await _userService.GetAsync(command.Email);
            var jwt = _jwtHandler.CreateToken(user.Id, user.Role);

            _cache.SetJwt(command.TokenId, jwt);
        }
    }
}