namespace StockAdvisor.Infrastructure.Commands.Investors
{
    public class DeleteFavouriteCompanyCommand : AuthenticatedCommandBase
    {
        public string CompanySymbol { get; set; } 
    }
}