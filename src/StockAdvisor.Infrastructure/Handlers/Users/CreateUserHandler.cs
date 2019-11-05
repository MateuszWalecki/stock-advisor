using System;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Services.TasksHandling;

namespace StockAdvisor.Infrastructure.Handlers.Users
{
    public class CreateUserHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IHandler _handler;
        private readonly IUserService _userService;

        public CreateUserHandler(IHandler handler, IUserService userService)
        {
            _handler = handler;
            _userService = userService;
        }

        public async Task HandleAsync(CreateUserCommand command)
            => await _handler
                .Run(async() =>
                {
                    var repositoryUser = await _userService.GetAsync(command.Email);

                    if (repositoryUser != null)
                    {
                        throw new EmailInUseSerExc($"Given email {command.Email} is already used.");
                    }
                })
                .OnSuccess(async () => await _userService.RegisterAsync(Guid.NewGuid(),
                    command.Email, command.FirstName, command.SurName, command.Password, UserRole.User))
                .Next()
                .ExecuteAllAsync();
    }
}