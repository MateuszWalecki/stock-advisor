using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.DTO;
using Xunit;

namespace StockAdvisor.Tests.EndToEnd.Controllers
{
    public class AccountControllerTests : ControllerTestBase
    {
        private readonly string _baseUrl = "account";
        public AccountControllerTests(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task change_password_on_unathorized_request_returns_unathorized_code()
        {
        //Given
            var client = Factory.CreateClient();
            var user = await AddUserWithoutInvestorToRepoAndGetAsync();

            var changePasswordCommand = new ChangeUserPasswordCommand()
            {
                CurrentPassword = user.Password,
                NewPassword = "sdafsadfgasdgdfag312423rf,"
            };

            var payload = GetPayload(changePasswordCommand);

        //When
            var response = await client.PutAsync(Url("password"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("sdgfasdfgsadf")]
        public async Task change_password_returns_unathorized_if_old_password_does_not_match(string oldPassword)
        {
        //Given
            HttpClient client = await CreateAuthorizedClient();

            var changePasswordCommand = new ChangeUserPasswordCommand()
            {
                CurrentPassword = oldPassword,
                NewPassword = "sdafsadfgasdgdfag312423rf,"
            };

            var payload = GetPayload(changePasswordCommand);

        //When
            var response = await client.PutAsync(Url("password"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task change_password_returns_bad_request_if_new_password_is_not_valid(string newPw)
        {
        //Given
            var user = await AddUserWithoutInvestorToRepoAndGetAsync();
            HttpClient client = await CreateAuthorizedClient(user);

            var changePasswordCommand = new ChangeUserPasswordCommand()
            {
                CurrentPassword = user.Password,
                NewPassword = newPw
            };

            var payload = GetPayload(changePasswordCommand);

        //When
            var response = await client.PutAsync(Url("password"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task change_password_without_body_returns_bad_request()
        {
        //Given
            var client = await CreateAuthorizedClient();

            var payload = GetPayload(null);

        //When
            var response = await client.PutAsync(Url("password"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task change_password_on_success_returns_no_content()
        {
        //Given
            var user = await AddUserWithoutInvestorToRepoAndGetAsync();
            HttpClient client = await CreateAuthorizedClient(user);

            var changePasswordCommand = new ChangeUserPasswordCommand()
            {
                CurrentPassword = user.Password,
                NewPassword = "sdafsadfgasdgdfag312423rf,"
            };

            var payload = GetPayload(changePasswordCommand);

        //When
            var response = await client.PutAsync(Url("password"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }


        [Fact]
        public async Task change_email_unathorized_request_sends_unathorized_code()
        {
        //Given
            var client = Factory.CreateClient();
            var user = await AddUserWithoutInvestorToRepoAndGetAsync();

            var changeEmailCommand = new ChangeUserEmailCommand()
            {
                Password = user.Password,
                NewEmail = "new_email@test.com"
            };

            var payload = GetPayload(changeEmailCommand);

        //When
            var response = await client.PutAsync(Url("email"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task change_email_returns_unathorized_if_password_does_not_match(string password)
        {
        //Given
            HttpClient client = await CreateAuthorizedClient();

            var changeEmailCommand = new ChangeUserEmailCommand()
            {
                Password = password,
                NewEmail = "new_email@test.com"
            };

            var payload = GetPayload(changeEmailCommand);

        //When
            var response = await client.PutAsync(Url("email"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("@test.com")]
        [InlineData("sample.email")]
        public async Task change_email_returns_bad_request_if_new_email_is_not_valid(string email)
        {
        //Given
            var user = await AddUserWithoutInvestorToRepoAndGetAsync();
            HttpClient client = await CreateAuthorizedClient(user);

            var changeEmailCommand = new ChangeUserEmailCommand()
            {
                Password = user.Password,
                NewEmail = email
            };

            var payload = GetPayload(changeEmailCommand);

        //When
            var response = await client.PutAsync(Url("email"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task change_email_without_body_returns_bad_request()
        {
        //Given
            var client = await CreateAuthorizedClient();

            var payload = GetPayload(null);

        //When
            var response = await client.PutAsync(Url("email"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task change_email_on_success_returns_no_content()
        {
        //Given
            var user = await AddUserWithoutInvestorToRepoAndGetAsync();
            HttpClient client = await CreateAuthorizedClient(user);

            var changeEmailCommand = new ChangeUserEmailCommand()
            {
                Password = user.Password,
                NewEmail = "newemail@test.com"
            };

            var payload = GetPayload(changeEmailCommand);

        //When
            var response = await client.PutAsync(Url("email"), payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task get_current_user_returns_unathorized_if_user_is_not_authorized()
        {
        //Given
            var client = Factory.CreateClient();
            
        //When
            var response = await client.GetAsync(Url("me"));
            var responseString = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(responseString);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
            user.Should().BeNull();
        }
        
        [Fact]
        public async Task get_current_user_returns_it_if_exists()
        {
        //Given
            var user = await AddUserWithInvestorToRepoAndGetAsync();
            HttpClient client = await CreateAuthorizedClient(user);
            
        //When
            var response = await client.GetAsync(Url("me"));
            var responseString = await response.Content.ReadAsStringAsync();
            var resultUser = JsonConvert.DeserializeObject<UserDto>(responseString);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            resultUser.Should().NotBeNull();
            Assert.Equal(user.Id, resultUser.Id);
            Assert.Equal(user.Email, resultUser.Email);
            Assert.Equal(user.FirstName, resultUser.FirstName);
            Assert.Equal(user.SurName, resultUser.SurName);
            Assert.Equal(user.Role.ToString(), resultUser.Role);
        }

        private string Url(string postifx)
            => _baseUrl + "/" + postifx;
    }
}