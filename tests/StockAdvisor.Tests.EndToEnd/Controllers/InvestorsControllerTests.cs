using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Investors;
using StockAdvisor.Infrastructure.DTO;
using Xunit;

namespace StockAdvisor.Tests.EndToEnd.Controllers
{
    public class InvestorsControllerTests : ControllerTestBase
    {
        private string _controllerRoute = "investors";

        public InvestorsControllerTests(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }
        
        [Fact]
        public async Task get_all_investors_returns_all_from_repo_with_status_ok()
        {
        //Given
            var client = Factory.CreateClient();

        //When
            var response = await client.GetAsync(Uri());
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
            var response = await client.GetAsync(Uri("me"));
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
            var client = await CreateAuthorizedClient();
            
        //When
            var response = await client.GetAsync(Uri("me"));
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
            var userWithoutInvestor = await AddUserWithoutInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(userWithoutInvestor);

        //When
            var response = await client.GetAsync(Uri("me"));
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
            var response = await client.PostAsync(Uri(), GetPayload(""));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task create_investor_returns_confict_if_proper_investor_exists()
        {
        //Given
            var user = await AddUserWithInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(user);
            
        //When
            var response = await client.PostAsync(Uri(), GetPayload(""));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task create_investor_returns_no_content_if_user_is_logged_and_does_not_contain_assigned_investor()
        {
        //Given
            var userWithoutInvestor = await AddUserWithoutInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(userWithoutInvestor);
            
        //When
            var response = await client.PostAsync(Uri(), GetPayload(""));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task delete_investor_on_unauthorized_user_returns_unathorized()
        {
        //Given
            var client = Factory.CreateClient();

        //When
            var response = await client.DeleteAsync(Uri("me"));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task delete_investor_returns_not_dound_if_related_investor_does_not_exist()
        {
        //Given
            dynamic user = await AddUserWithoutInvestorToRepoAndGetAsync();
            HttpClient client = await CreateAuthorizedClient(user);

        //When
            var response = await client.DeleteAsync(Uri("me"));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task delete_investor_returns_no_content()
        {
        //Given
            var client = await CreateAuthorizedClient();

        //When
            var response = await client.DeleteAsync(Uri("me"));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task add_favourite_company_returns_unathorized_if_user_us_not_authorized()
        {
        //Given
            var companySymbol = "AAPL";
            var client = Factory.CreateClient();
            var payload = GetPayload(companySymbol);

        //When
            var response = await client.PostAsync(Uri("companies"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("INVALID_SYMBOL")]
        [InlineData(null)]
        [InlineData("")]
        public async Task add_favourite_company_returns_bad_request_if_given_company_symbol_is_incorrect(string symbol)
        {
        //Given
            var user = await AddUserWithInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(user);

            var command = new AddFavouriteCompanyCommand()
            {
                CompanySymbol = symbol
            };
            var payload = GetPayload(command);

        //When
            var response = await client.PostAsync(Uri("companies"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task add_favourite_company_returns_bad_request_if_body_request_is_empty()
        {
        //Given
            var client = await CreateAuthorizedClient();
            var payload = GetPayload(null);

        //When
            var response = await client.PostAsync(Uri("companies"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task add_favourite_company_returns_no_content_on_success()
        {
        //Given
            var user = await AddUserWithInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(user);

            string companySymbol = "MSFT";
            var command = new AddFavouriteCompanyCommand()
            {
                CompanySymbol = companySymbol
            };
            var payload = GetPayload(command);

        //When
            var response = await client.PostAsync(Uri("companies"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task add_favourite_company_returns_conflict_if_given_symbol_is_present()
        {
        //Given
            var user = await AddUserWithInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(user);

            string companySymbol = "AAPL";
            var command = new AddFavouriteCompanyCommand()
            {
                CompanySymbol = companySymbol
            };
            var payload = GetPayload(command);

        //When
            var response = await client.PostAsync(Uri("companies"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Conflict);
        }


        [Fact]
        public async Task remove_favourite_company_returns_unathorized_if_user_is_not_authorized()
        {
        //Given
            var companySymbol = "AAPL";
            var client = Factory.CreateClient();

        //When
            var response = await client.DeleteAsync(Uri($"companies/{companySymbol}"));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task remove_favourite_company_returns_bad_request_if_symbol_is_not_in_collection()
        {
        //Given
            var userWithInvestor = await AddUserWithInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(userWithInvestor);
            var companySymbol = "MSFT";

        //When
            var response = await client.DeleteAsync(Uri($"companies/{companySymbol}"));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task remove_favourite_company_returns_no_content_on_success()
        {
        //Given
            var userWithInvestor = await AddUserWithInvestorToRepoAndGetAsync();
            var client = await CreateAuthorizedClient(userWithInvestor);

            var companySymbol = "AAPL";

        //When
            var response = await client.DeleteAsync(Uri($"companies/{companySymbol}"));

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        private string Uri(string subRoute = null)
            => string.IsNullOrEmpty(subRoute) ?
                _controllerRoute :
                _controllerRoute + "/" + subRoute;
    }
}