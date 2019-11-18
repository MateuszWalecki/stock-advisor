using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using StockAdvisor.Api;
using Xunit;

namespace StockAdvisor.Tests.EndToEnd.Controllers
{
    public class CompaniesControllerTests : ControllerTestBase
    {
        private string _controllerRoute = "companies";

        public CompaniesControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task get_all_companies_returns_unathorized_if_use_is_not_authorized()
        {
        //Given
            var client = Factory.CreateClient();
            
        //When
            var response = await client.GetAsync(Uri());
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task get_all_companies_returns_ok_status()
        {
        //Given
            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync(Uri());
            
        //Then
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task get_company_historical_returns_unathorized_if_use_is_not_authorized()
        {
        //Given
            var client = Factory.CreateClient();
            
        //When
            var response = await client.GetAsync(Uri("AAPL"));
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task get_company_historical_returns_ok_status()
        {
        //Given
            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync(Uri("AAPL"));
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task predict_values_return_unathorized_on_unlogged_user()
        {
        //Given
            string companySymbol = "AAPL",
                algortihmName = "linear";

            var client = Factory.CreateClient();
            
        //When
            var response = await client.GetAsync(PredictUri(companySymbol, algortihmName));
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task predict_values_return_ok_status()
        {
        //Given
            string companySymbol = "AAPL",
                algortihmName = "linear";

            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync(PredictUri(companySymbol, algortihmName));
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [Fact]
        public async Task predict_values_returns_bad_request_if_algorithm_is_invalid()
        {
        //Given
            string companySymbol = "AAPL",
                invalidAlgorithm = "invalid";

            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync(PredictUri(companySymbol, invalidAlgorithm));
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task predict_values_returns_bad_request_if_company_symbol_is_invalid()
        {
        //Given
            string invalidCompanySymbol = "invalid",
                algortihmName = "linear";

            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync(PredictUri(invalidCompanySymbol, algortihmName));
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        private string PredictUri(string companySymbol, string algorithm)
            => Uri($"predict?CompanySymbol={companySymbol}&Algorithm={algorithm}");

        private string Uri(string subRoute = null)
            => string.IsNullOrEmpty(subRoute) ?
                _controllerRoute :
                _controllerRoute + "/" + subRoute;
    }
}