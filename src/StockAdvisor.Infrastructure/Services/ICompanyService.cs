using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface ICompanyService : IService
    {
        Task<CompanyValueStatusDto> GetValueStatusAsync(string companySymbol);
        Task<IEnumerable<CompanyDto>> BrowseAsync();
        Task<CompanyValueStatusDto> PredictValues(string companySymbol,
            string algorithm);
    }
}