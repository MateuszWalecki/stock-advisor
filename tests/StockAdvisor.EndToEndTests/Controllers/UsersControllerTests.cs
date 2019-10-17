using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.DTO;
using Xunit;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class UsersControllerTests : ControllerTestBase
    {
        [Fact]
        public async Task given_valid_email_user_is_returned()
        {
            //Given
            var email = "first@example.com";

            //When
            var response = await Client.GetAsync($"users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(responseString);
            
            //Then
            response.EnsureSuccessStatusCode();
            user.Email.Should().BeEquivalentTo(email);
        }

        [Fact]
        public async Task given_invalid_email_user_is_not_returned()
        {
            //Given
            var email = "bademail@example.com";

            //When
            var response = await Client.GetAsync($"users/{email}");
            
            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task given_unused_email_user_is_created_and_user_location_is_returen_with_status_created()
        {
            //Given
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
            var response = await Client.PostAsync("users", payload);
            var resopnseString = await response.Content.ReadAsStringAsync();

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Created);
            response.Headers.Location.AbsolutePath.Should().BeEquivalentTo($"/users/{email}");

            var user = await GetUserAsync(email);
            user.Email.Should().BeEquivalentTo(email);
        }

        [Fact]
        public async Task trying_to_create_user_with_currently_used_email_returns_conflict_status()
        {
            //Given
            string usedEmail = "first@example.com";
            var creauteUserRequest = new CreateUserCommand
            {
                Email = usedEmail,
                FirstName = "Name",
                SurName = "Surname",
                Password = "Secret1"
            };
            var payload = GetPayload(creauteUserRequest);

            //When
            var response = await Client.PostAsync("users", payload);

            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Conflict);
            response.Headers.Location.Should().BeNull();
        }

        private async Task<UserDto> GetUserAsync(string email)
        {
            var response = await Client.GetAsync($"users/{email}");
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(responseString); 
        }
    }
}