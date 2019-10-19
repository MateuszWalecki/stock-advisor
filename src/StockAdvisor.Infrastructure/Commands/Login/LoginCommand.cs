namespace StockAdvisor.Infrastructure.Commands.Login
{
    public class LoginCommand : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}