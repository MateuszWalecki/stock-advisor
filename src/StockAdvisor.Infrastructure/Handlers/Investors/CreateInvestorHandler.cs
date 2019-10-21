using System.Threading.Tasks;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Investors;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Investors
{
    public class CreateInvestorHandler : ICommandHandler<CreateInvestorCommand>
    {
        private readonly IInvestorService _investorService;
        public CreateInvestorHandler(IInvestorService investorService)
        {
            _investorService = investorService;
        }

        public async Task HandleAsync(CreateInvestorCommand command)
        {
            await _investorService.RegisterAsync(command.UserId);
        }
    }
}