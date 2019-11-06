using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.DTO;
using Xunit;
using Xunit.Abstractions;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class UsersControllerTests : ControllerTestBase
    {
        public UsersControllerTests(WebApplicationFactory<Startup> factory,
            ITestOutputHelper output)
            : base(factory, output)
        {
        }
        
        [Fact]
        public async Task on_unathorized_get_user_request_unathorized_response_is_send()
        {
        //Given
            var client = Factory.CreateClient();

            dynamic user = await AddUserWithInvestorToRepoAndGetAsync();

        //When
            var response = await client.GetAsync($"users/{user.Email}");
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }        

        [Fact]
        public async Task given_valid_email_user_is_returned()
        {
        //Given
            var client = await CreateAuthorizedClient();

            dynamic user = await AddUserWithInvestorToRepoAndGetAsync();

        //When
            var response = await client.GetAsync($"users/{user.Email}");
            var responseString = await response.Content.ReadAsStringAsync();
            var returnedEser = JsonConvert.DeserializeObject<UserDto>(responseString);
            
        //Then
            response.EnsureSuccessStatusCode();
            returnedEser.Email.Should().BeEquivalentTo(user.Email);
        }

        [Fact]
        public async Task given_invalid_email_user_is_not_returned()
        {
        //Given
            var client = await CreateAuthorizedClient();
            var email = "bad@bad.email";

        //When
            var response = await client.GetAsync($"users/{email}");
            
        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task given_unused_email_user_is_created_and_user_location_is_returen_with_status_created()
        {
        //Given
            var client = await CreateAuthorizedClient();
            
            string email = "new_user@email.com",
	            firstName = "Luis",
	            surName = "Suarez",
	            password = "FCBarcelona1";

            var creauteUserRequest = new CreateUserCommand
            {
                Email = email,
                FirstName = firstName,
                SurName = surName,
                Password = password
            };
            
            var payload = GetPayload(creauteUserRequest);

        //When
            var response = await client.PostAsync("users", payload);
            var resopnseString = await response.Content.ReadAsStringAsync();

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.AbsolutePath.Should().BeEquivalentTo($"/users/{email}");

            var user = await GetUserAsync(client, email);
            user.Email.Should().BeEquivalentTo(email);
        }

        [Fact]
        public async Task trying_to_create_user_with_currently_used_email_returns_conflict_status()
        {
        //Given
            var client = Factory.CreateClient();
            dynamic existingUser = await AddUserWithInvestorToRepoAndGetAsync();

            string usedEmail = existingUser.Email;
            var creauteUserRequest = new CreateUserCommand
            {
                Email = usedEmail,
                FirstName = "Name",
                SurName = "Surname",
                Password = "Secret1"
            };
            var payload = GetPayload(creauteUserRequest);

        //When
            var response = await client.PostAsync("users", payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Conflict);
            response.Headers.Location.Should().BeNull();
        }

        [Fact]
        public async Task get_all_users_returns_all_from_repo_with_status_ok()
        {
        //Given
            var client = Factory.CreateClient();

        //When
            var response = await client.GetAsync("users");
            var responseString = await response.Content.ReadAsStringAsync();

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            responseString.Should().NotBeNullOrEmpty();
        }

        private static async Task<UserDto> GetUserAsync(HttpClient client,
            string email)
        {
            var response = await client.GetAsync($"users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(responseString); 
        }
    }
}