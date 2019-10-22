using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;

namespace StockAdvisor.Infrastructure.Repositories
{
    public class FinancialModelingPrepRepository : ICompanyRepository
    {
        private readonly HttpClient _client;

        public FinancialModelingPrepRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Company>> BrowseAsync()
        {
            var response = await _client.GetAsync("api/v3/company/stock/list");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            var companies = JsonConvert.DeserializeObject<CompaniesJson>(stringResponse);
            return companies.SymbolsList;
        }

        public Task<Company> GetAsync(string companySymbol)
        {
            throw new System.NotImplementedException();
        }

        private class CompaniesJson
        {
            public List<Company> SymbolsList { get; set; }
        }
    }
}