using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class InMemoryStockRepository : IStockMarketDataRepository
    {
        private static readonly ISet<Company> _companies = new HashSet<Company>
        {
            new Company("AAPL", "Apple Inc."),
            new Company("MSFT", "Microsoft Corporation"),
            new Company("AMZN", "Amazon.com Inc.")
        };

        private static Dictionary<string, IEnumerable<StockValue>> _values = 
            new Dictionary<string, IEnumerable<StockValue>>()
            {
                {"AAPL", new List<StockValue>()
                    {
                        new StockValue(DateTime.Now.AddDays(-4), 227.03m),
                        new StockValue(DateTime.Now.AddDays(-3), 230.09m),
                        new StockValue(DateTime.Now.AddDays(-2), 236.21m),
                        new StockValue(DateTime.Now.AddDays(-1), 235.87m),
                        new StockValue(DateTime.Now, 235.52m)
                    }},
                {"MSFT", new List<StockValue>()
                    {
                        new StockValue(DateTime.Now.AddDays(-4), 138.24m),
                        new StockValue(DateTime.Now.AddDays(-3), 139.1m),
                        new StockValue(DateTime.Now.AddDays(-2), 139.68m),
                        new StockValue(DateTime.Now.AddDays(-1), 139.55m),
                        new StockValue(DateTime.Now, 141.01m)
                    }},
                {"AMZN", new List<StockValue>()
                    {
                        new StockValue(DateTime.Now.AddDays(-4), 1721.99m),
                        new StockValue(DateTime.Now.AddDays(-3), 1720.26m),
                        new StockValue(DateTime.Now.AddDays(-2), 1761.92m),
                        new StockValue(DateTime.Now.AddDays(-1), 1736.43m),
                        new StockValue(DateTime.Now, 1742.01m)
                    }},
            };

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
            => await Task.FromResult(_companies);

        public async Task<IEnumerable<StockValue>> GetCompanyValueHistoryAsync(string companySymbol)
        {
            var stockValues = _values[companySymbol];

            if (stockValues == null)
            {
                throw new ServiceException(ErrorCodes.CompanyNotFound,
                    $"Given company cannot be found. Symbol: {companySymbol}.");
            }

            return await Task.FromResult(stockValues);
        }
    }
}