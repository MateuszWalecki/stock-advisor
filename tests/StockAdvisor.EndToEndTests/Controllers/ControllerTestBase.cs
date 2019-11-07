using System;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StockAdvisor.Api;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.Commands.Login;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Services.DataInitializer;
using Xunit;
using Xunit.Abstractions;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class ControllerTestBase
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        protected readonly WebApplicationFactory<Startup> Factory;
        
        protected ControllerTestBase(WebApplicationFactory<Startup> factory)
        {
            Factory = SetPathToTestAppSettings(factory);
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


        private WebApplicationFactory<Startup> SetPathToTestAppSettings(
            WebApplicationFactory<Startup> factory)
        {
            var projectDir = GetProjectPath("", typeof(ControllerTestBase).GetTypeInfo().Assembly);
            var configPath = Path.Combine(projectDir, "appsettings.json");
            
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context,conf) =>
                {
                    conf.AddJsonFile(configPath);
                });
            });
        }

        /// Ref: https://stackoverflow.com/a/52136848/3634867
        /// <summary>
        /// Gets the full path to the target project that we wish to test
        /// </summary>
        /// <param name="projectRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = System.AppContext.BaseDirectory;

            // Find the path to the target project
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                    if (projectFileInfo.Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }
    }
}