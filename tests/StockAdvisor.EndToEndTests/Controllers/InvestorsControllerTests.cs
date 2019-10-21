using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.DTO;
using Xunit;
using Xunit.Abstractions;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class InvestorsControllerTests : ControllerTestBase
    {
        public InvestorsControllerTests(WebApplicationFactory<Startup> factory,
            ITestOutputHelper output)
            : base(factory, output)
        {
        }
        
        [Fact]
        public async Task get_all_investors_returns_all_from_repo_with_status_ok()
        {
            //Given
            var client = Factory.CreateClient();

            //When
            var response = await client.GetAsync("investors");
            var responseString = await response.Content.ReadAsStringAsync();

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseString.Should().NotBeNullOrEmpty();
        }
        
        [Fact]
        public async Task get_investor_by_email_returns_it_if_exists()
        {
            //Given
            string validEmail = "user1@test.com";
            var client = Factory.CreateClient();

            //When
            var response = await client.GetAsync($"investors/{validEmail}");
            var responseString = await response.Content.ReadAsStringAsync();
            var investor = JsonConvert.DeserializeObject<InvestorDto>(responseString);

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            investor.Should().NotBeNull();
        }
        
        [Fact]
        public async Task get_investor_by_email_returns_null_if_does_not_exist()
        {
            //Given
            string unvalidEmail = "bad@tests.com";
            var client = Factory.CreateClient();

            //When
            var response = await client.GetAsync($"investors/{unvalidEmail}");
            var responseString = await response.Content.ReadAsStringAsync();

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}