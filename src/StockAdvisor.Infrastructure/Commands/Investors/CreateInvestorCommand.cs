using System;

namespace StockAdvisor.Infrastructure.Commands.Investors
{
    public class CreateInvestorCommand : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
    }
}