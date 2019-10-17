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
            
            // TODO: do change password logic
            // var user = await _userService.GetAsync(command.Email);

            // if (user == null)
            // {
            //     throw new ServiceException(ServiceErrorCodes.UserNotFound,
            //         $"User cannot be found by the email {command.Email}.");
            // }

            // await _userService.ChangeUserPasswordAsync(user.Id,
            //     command.NewPassword, command.CurrentPassword);
        }
    }
}