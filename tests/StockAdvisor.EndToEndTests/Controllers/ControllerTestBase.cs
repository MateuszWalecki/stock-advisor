using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.DTO;
using Xunit;
using Xunit.Abstractions;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class ControllerTestBase
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> Factory;
        protected readonly ITestOutputHelper Output;

        protected ControllerTestBase(WebApplicationFactory<Startup> factory,
            ITestOutputHelper output)
        {
            Factory = factory;
            Output = output;
        }

        
        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        protected static async Task<string> GetValidBearerTokenHeader(HttpClient client,
            string email, string password)
        {
            var command = new LoginCommand
            {
                Email = email,
                Password = password    
            };
            var payload = GetPayload(command);

            var response = await client.PostAsync("/login", payload);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var jwtToken = JsonConvert.DeserializeObject<JwtDto>(stringResponse);

            return "Bearer " + jwtToken.Token;
        }

        public async Task<HttpClient> CreateAuthorizedClient(
            string email = "user1@test.com", string password = "Secret1")
        {
            var client = Factory.CreateClient();

            var authorizationHeader =  await GetValidBearerTokenHeader(client, email, password);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            return client;
        }
    }
}