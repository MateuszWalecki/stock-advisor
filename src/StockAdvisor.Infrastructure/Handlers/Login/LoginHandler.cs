using System.Threading.Tasks;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand>
    {
        private readonly IUserService _userService;
        
        public LoginHandler(IUserService userService)
        {
            _userService = userService;
        } 

        public async Task HandleAsync(LoginCommand command)
        {
            await _userService.LoginAsync(command.Email, command.Password);
        }
    }
}