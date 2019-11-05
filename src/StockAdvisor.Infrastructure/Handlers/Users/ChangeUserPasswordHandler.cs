using System.Threading.Tasks;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Users
{
    public class ChangeUserPasswordHandler : ICommandHandler<ChangeUserPasswordCommand>
    {
        private readonly IUserService _userService;
        public ChangeUserPasswordHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(ChangeUserPasswordCommand command)
        {
            await Task.CompletedTask;
            
            var user = await _userService.GetAsync(command.UserId);

            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    $"User cannot be found using id {command.UserId}.");
            }

            await _userService.ChangeUserPasswordAsync(command.UserId,
                command.NewPassword, command.CurrentPassword);
        }
    }
}