using StockAdvisor.Infrastructure.Commands;

namespace StockAdvisor.Infrastructure.Commands.Users
{
    public class ChangeUserPasswordCommand : ICommand
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}