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

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class AccountControllerTests : ControllerTestBase
    {
        public AccountControllerTests(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task given_valid_current_password_and_new_password_current_should_be_changed()
        {
            //Given
            var client = _factory.CreateClient();
            var command = new ChangeUserPasswordCommand()
            {
                Email = "new_email@example.com",
                CurrentPassword = "secret",
                NewPassword = "secret2"
            };
            var payload = GetPayload(command);

            //When
            var response = await client.PutAsync("account/password", payload);
            
            //Then
            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }
    }
}