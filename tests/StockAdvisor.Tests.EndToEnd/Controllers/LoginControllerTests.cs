using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.DTO;
using Xunit;
using Xunit.Abstractions;

namespace StockAdvisor.Tests.EndToEnd.Controllers
{
    public class LoginControllerTests : ControllerTestBase
    {
        public LoginControllerTests(WebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task given_valid_credentials_ok_status_with_jwt_are_returned()
        {
        //Given
            var client = Factory.CreateClient();
            dynamic user = await AddUserWithInvestorToRepoAndGetAsync();

            var command = new LoginCommand
            {
                Email = user.Email,
                Password = user.Password    
            };
            var payload = GetPayload(command);

        //When
            var response = await client.PostAsync("/login", payload);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var jwtToken = JsonConvert.DeserializeObject<JwtDto>(stringResponse);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
            jwtToken.Should().NotBeNull();
            jwtToken.Token.Should().NotBeNullOrEmpty();
            jwtToken.ExpiryTicks.Should().NotBe(0);
        }

        [Fact]
        public async Task given_invalid_email_unathorized_code_is_returned()
        {
        //Given
            var client = Factory.CreateClient();
            dynamic user = await AddUserWithInvestorToRepoAndGetAsync();

            var command = new LoginCommand
            {
                Email = "wrong_email",
                Password = user.Password 
            };
            var payload = GetPayload(command);

        //When
            var response = await client.PostAsync("/login", payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task given_invalid_password_unathorized_code_is_returned()
        {
        //Given
            var client = Factory.CreateClient();
            dynamic user = await AddUserWithInvestorToRepoAndGetAsync();

            var command = new LoginCommand
            {
                Email = user.Email,
                Password = "invalid password"    
            };
            var payload = GetPayload(command);

        //When
            var response = await client.PostAsync("/login", payload);

        //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.Unauthorized);
        }
    }
}