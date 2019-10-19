using System;

namespace StockAdvisor.Infrastructure.Commands.Login
{
    public class LoginCommand : ICommand
    {
        public Guid TokenId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}