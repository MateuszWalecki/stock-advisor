using System;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Users
{
    public class CreateUserHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUserService _userService;
        public CreateUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task HandleAsync(CreateUserCommand command)
        {
            var repositoryUser = await _userService.GetAsync(command.Email);

            if (repositoryUser != null)
            {
                throw new ServiceException(ErrorCodes.EmailInUse,
                    $"Given email {command.Email} is already used.");
            }

            await _userService.RegisterAsync(Guid.NewGuid(), command.Email, command.FirstName, command.SurName,
                command.Password, UserRole.User);
        }
    }
}