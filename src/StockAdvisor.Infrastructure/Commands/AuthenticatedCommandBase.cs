using System;

namespace StockAdvisor.Infrastructure.Commands
{
    public class AuthenticatedCommandBase : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
    }
}