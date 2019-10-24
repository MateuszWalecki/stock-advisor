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
        public async Task get_current_investor_requires_authorization()
        {
            //Given
            var client = Factory.CreateClient();
            
            //When
            var response = await client.GetAsync($"investors/current");
            var responseString = await response.Content.ReadAsStringAsync();
            var investor = JsonConvert.DeserializeObject<InvestorDto>(responseString);

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
            investor.Should().BeNull();
        }
        
        [Fact]
        public async Task get_current_investor_returns_it_if_exists()
        {
            //Given
            var client = Factory.CreateClient();
            var authorizationHeader =  await GetValidBearerTokenHeader(client);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
            
            //When
            var response = await client.GetAsync($"investors/current");
            var responseString = await response.Content.ReadAsStringAsync();
            var investor = JsonConvert.DeserializeObject<InvestorDto>(responseString);

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            investor.Should().NotBeNull();
        }
        
        [Fact]
        public async Task get_current_investor_returns_null_if_does_not_exist()
        {
            //Given
            var client = Factory.CreateClient();
            var authorizationHeader =  await GetValidBearerTokenHeader(client, "without_investor1@test.com");
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            //When
            var response = await client.GetAsync($"investors/current");
            var responseString = await response.Content.ReadAsStringAsync();

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }
        

        [Fact]
        public async Task create_investor_returns_unathorized_if_user_is_not_logged_in()
        {
            //Given
            var client = Factory.CreateClient();
            
            //When
            var response = await client.PostAsync($"investors", GetPayload(""));

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task create_investor_returns_no_content_if_user_is_logged_and_does_not_contain_assigned_investor()
        {
            //Given
            var client = Factory.CreateClient();
            var authorizationHeader =  await GetValidBearerTokenHeader(client, "without_investor2@test.com", "Secret2");
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
            
            //When
            var response = await client.PostAsync($"investors", GetPayload(""));

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }
    }
}