using System.Threading.Tasks;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Investors;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Investors
{
    public class DeleteInvestorHandler : ICommandHandler<DeleteInvestorCommand>
    {
        private readonly IInvestorService _investorService;
        
        public DeleteInvestorHandler(IInvestorService investorService)
        {
            _investorService = investorService;
        }
        
        public async Task HandleAsync(DeleteInvestorCommand command)
        {
            await _investorService.RemoveAsync(command.UserId);
        }
    }
}