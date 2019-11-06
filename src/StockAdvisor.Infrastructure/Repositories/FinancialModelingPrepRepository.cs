using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class FinancialModelingPrepRepository : ICompanyRepository
    {
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;
        private readonly static string CacheKey = "companies";

        public FinancialModelingPrepRepository(HttpClient client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<IEnumerable<Company>> BrowseAsync()
        {
            var companies = _cache.Get<IEnumerable<Company>>(CacheKey);

            if (companies == null)
            {
                companies = await GetCompaniesFromExternalSource();

                _cache.Set(CacheKey, companies);
            }

            return companies;
        }

        public async Task<CompanyValueStatus> GetCompanyValueStatusAsync(
            string companySymbol)
        {
            var historicalPrice = 
                _cache.Get<IEnumerable<CompanyValue>>(GetStrictCompanyKey(companySymbol));

            if (historicalPrice == null)
            {
                historicalPrice = await GetHistoricalPriceFromExternalSource(companySymbol);

                _cache.Set(GetStrictCompanyKey(companySymbol), historicalPrice);
            }

            var companiesInfo = await BrowseAsync();
            var companyInfo = companiesInfo.SingleOrDefault(x => x.Symbol == companySymbol);
            
            return new CompanyValueStatus(companyInfo, historicalPrice);
        }

        private string GetStrictCompanyKey(string companySymbol)
            => $"{CacheKey}-{companySymbol}";

        private async Task<IEnumerable<Company>> GetCompaniesFromExternalSource()
        {
            var response = await _client.GetAsync("api/v3/company/stock/list");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var jsonCompanies = JsonConvert.DeserializeObject<CompaniesJson>(stringResponse);
            return jsonCompanies.SymbolsList;
        }

        private async Task<IEnumerable<CompanyValue>> GetHistoricalPriceFromExternalSource(
            string companySymbol)
        {
            var response = await _client.GetAsync(
                $"/api/v3/historical-price-full/{companySymbol}?serietype=line");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var jsonHistorical = JsonConvert.DeserializeObject<HistoricalJson>(stringResponse);
            return jsonHistorical.Historical;
        }
        
        private class CompaniesJson
        {
            public IEnumerable<Company> SymbolsList { get; set; }
        }

        private class HistoricalJson
        {
            public string Symbol { get; set; }
            public IEnumerable<CompanyValue> Historical { get; set; }
        }
    }
}