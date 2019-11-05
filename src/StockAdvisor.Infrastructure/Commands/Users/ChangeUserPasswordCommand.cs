using StockAdvisor.Infrastructure.Commands;

namespace StockAdvisor.Infrastructure.Commands.Users
{
    public class ChangeUserPasswordCommand : AuthenticatedCommandBase
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}