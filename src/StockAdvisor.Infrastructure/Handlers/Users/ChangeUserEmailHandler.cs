using System.Threading.Tasks;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Services.TasksHandling;

namespace StockAdvisor.Infrastructure.Handlers.Users
{
    public class ChangeUserEmailHandler : ICommandHandler<ChangeUserEmailCommand>
    {
        private readonly IHandler _handler;
        private readonly IUserService _userService;

        public ChangeUserEmailHandler(IHandler handler, IUserService userService)
        {
            _handler = handler;
            _userService = userService;
        }

        public async Task HandleAsync(ChangeUserEmailCommand command)
            => await _handler
                .Run(async () =>
                {
                    var user = await _userService.GetAsync(command.UserId);

                    if (user == null)
                    {
                        throw new UserNotFoundSerExc($"User cannot be found using id {command.UserId}.");
                    }
                })
                .OnSuccess(async () => 
                    await _userService.ChangeUserEmailAsync(command.UserId,
                        command.CurrentPassword, command.NewEmail))
                .Next()
                .ExecuteAllAsync();
    }
}