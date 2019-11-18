namespace StockAdvisor.Infrastructure.Commands.Investors
{
    public class AddFavouriteCompanyCommand : AuthenticatedCommandBase
    {
        public string CompanySymbol { get; set; } 
    }
}