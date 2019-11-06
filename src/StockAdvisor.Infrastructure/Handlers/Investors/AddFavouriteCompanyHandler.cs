using System.Threading.Tasks;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Commands.Investors;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Infrastructure.Handlers.Investors
{
    public class AddFavouriteCompanyHandler : ICommandHandler<AddFavouriteCompanyCommand>
    {
        private readonly IInvestorService _investorService;
        
        public AddFavouriteCompanyHandler(IInvestorService investorService)
        {
            _investorService = investorService;
        }
        
        public async Task HandleAsync(AddFavouriteCompanyCommand command)
        {
            await _investorService.AddToFavouriteCompaniesAsync(
                command.UserId, command.CompanySymbol);
        }
    }
}