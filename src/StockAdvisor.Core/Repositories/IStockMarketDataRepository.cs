using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Core.Repositories
{
    public interface IStockMarketDataRepository : IRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<IEnumerable<StockValue>> GetCompanyValueHistoryAsync(string companySymbol);
    }
}