using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface ICompanyService : IService
    {
        Task<CompanyDto> GetAsync(string companySymbol);
        Task<IEnumerable<CompanyDto>> BrowseAsync();
    }
}