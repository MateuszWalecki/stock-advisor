using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface ICompanyService : IService
    {
        Task<IEnumerable<CompanyPriceDto>> GetHistoricalAsync(string companySymbol);
        Task<IEnumerable<CompanyDto>> BrowseAsync();
    }
}