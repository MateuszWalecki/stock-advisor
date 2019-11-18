using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Services.DataInitializer;
using StockAdvisor.Tests.EndToEnd.Configuration;
using Xunit;

namespace StockAdvisor.Tests.EndToEnd.Controllers
{
    public class ControllerTestBase
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> Factory;
        
        protected ControllerTestBase(WebApplicationFactory<Startup> factory)
        {
            Factory = factory.ConfigureFactory();
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
            ExpandoObject userToAuthorize = null)
        {
            var client = Factory.CreateClient();
            
            dynamic userData = 
                userToAuthorize == null ?
                    await AddUserWithInvestorToRepoAndGetAsync() :
                    userToAuthorize as dynamic;

            var authorizationHeader =  await GetValidBearerTokenHeader(client, (string)userData.Email,
                (string)userData.Password);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            return client;
        }

        public async Task<ExpandoObject> AddUserWithInvestorToRepoAndGetAsync()
        {
            var dataInitializer =
                Factory.Server.Services.GetService(typeof(IDataInitializer)) as IDataInitializer;

            return await dataInitializer.AddAndGetNextUserWithInvestor();
        }

        public async Task<ExpandoObject> AddUserWithoutInvestorToRepoAndGetAsync()
        {
            var dataInitializer =
                Factory.Server.Services.GetService(typeof(IDataInitializer)) as IDataInitializer;

            return await dataInitializer.AddAndGetNextUserWithoutInvestor();
        }
    }
}