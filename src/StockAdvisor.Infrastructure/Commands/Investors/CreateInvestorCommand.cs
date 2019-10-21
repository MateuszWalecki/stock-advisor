using System;

namespace StockAdvisor.Infrastructure.Commands.Investors
{
    public class CreateInvestorCommand : ICommand
    {
        public Guid UserId { get; set; }
    }
}