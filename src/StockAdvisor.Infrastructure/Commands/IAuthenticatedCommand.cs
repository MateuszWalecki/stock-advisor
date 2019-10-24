using System;

namespace StockAdvisor.Infrastructure.Commands
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid UserId { get; set; }
    }
}