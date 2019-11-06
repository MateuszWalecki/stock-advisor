namespace StockAdvisor.Infrastructure.Commands.Users
{
    public class ChangeUserEmailCommand : AuthenticatedCommandBase
    {
        public string CurrentPassword { get; set; }
        public string NewEmail { get; set; }
    }
}