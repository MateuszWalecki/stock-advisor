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

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class LoginControllerTests : ControllerTestBase
    {
        private readonly string _validEmail = "user1@test.com",
            _validPassword = "Secret1";

        public LoginControllerTests(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task given_valid_credentials_ok_status_with_jwt_are_returned()
        {
            //Given
            var client = _factory.CreateClient();

            var command = new LoginCommand
            {
                Email = _validEmail,
                Password = _validPassword    
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
            var client = _factory.CreateClient();

            var command = new LoginCommand
            {
                Email = "wrong_email",
                Password = _validPassword    
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
            var client = _factory.CreateClient();

            var command = new LoginCommand
            {
                Email = _validEmail,
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