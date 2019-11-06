using System.Threading.Tasks;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Investors;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Investors
{
    public class DeleteFavouriteCompanyHandler : ICommandHandler<DeleteFavouriteCompanyCommand>
    {
        private readonly IInvestorService _investorService;
        
        public DeleteFavouriteCompanyHandler(IInvestorService investorService)
        {
            _investorService = investorService;
        }

        public async Task HandleAsync(DeleteFavouriteCompanyCommand command)
        {
            await _investorService.RemoveFromFavouriteCompaniesAsync(
                command.UserId, command.CompanySymbol);
        }
    }
}