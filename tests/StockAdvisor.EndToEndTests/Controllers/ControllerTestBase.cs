using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.DTO;
using Xunit;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class ControllerTestBase
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> _factory;

        protected ControllerTestBase(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        
        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        

        protected async Task<string> GetValidBearerTokenHeader(HttpClient client)
        {
            var command = new LoginCommand
            {
                Email = "first@example.com",
                Password = "Password1"    
            };
            var payload = GetPayload(command);

            var response = await client.PostAsync("/login", payload);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var jwtToken = JsonConvert.DeserializeObject<JwtDto>(stringResponse);

            return "Bearer " + jwtToken.Token;
        }
    }
}