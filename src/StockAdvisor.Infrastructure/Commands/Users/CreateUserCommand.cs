namespace StockAdvisor.Infrastructure.Commands.Users
{
    public class CreateUserCommand : ICommand
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Password { get; set; }
    }
}