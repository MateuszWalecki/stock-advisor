namespace StockAdvisor.Infrastructure.Commands.Users
{
    public class ChangeUserEmailCommand : AuthenticatedCommandBase
    {
        public string Password { get; set; }
        public string NewEmail { get; set; }
    }
}