using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using StockAdvisor.Api;
using Xunit;
using Xunit.Abstractions;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class CompaniesControllerTests : ControllerTestBase
    {
        public CompaniesControllerTests(WebApplicationFactory<Startup> factory,
            ITestOutputHelper output) : base(factory, output)
        {
        }

        [Fact]
        public async Task get_all_companies_returns_unathorized_if_use_is_not_authorized()
        {
        //Given
            var client = Factory.CreateClient();
            
        //When
            var response = await client.GetAsync("Companies");
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }
        
        [Fact]
        public async Task get_all_companies_returns_ok_status()
        {
        //Given
            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync("Companies");
            
        //Then
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task get_company_historical_returns_unathorized_if_use_is_not_authorized()
        {
        //Given
            var client = Factory.CreateClient();
            
        //When
            var response = await client.GetAsync("Companies/AAPL");
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task get_company_historical_returns_ok_status()
        {
        //Given
            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync("Companies/AAPL");
            
        //Then
            response.EnsureSuccessStatusCode();
        }
    }
}