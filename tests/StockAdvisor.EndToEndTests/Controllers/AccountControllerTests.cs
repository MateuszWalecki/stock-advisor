using System;
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
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.Commands.Users;
using StockAdvisor.Infrastructure.DTO;
using Xunit;
using Xunit.Abstractions;

namespace StockAdvisor.EndToEndTests.Controllers
{
    public class AccountControllerTests : ControllerTestBase
    {
        public AccountControllerTests(WebApplicationFactory<Startup> factory,
            ITestOutputHelper output)
            : base(factory, output)
        {
        }
    //TODO: test account cotroller
    //     [Fact]
    //     public async Task given_valid_current_password_and_new_password_current_should_be_changed()
    //     {
    //     //Given

    //     //When
            
    //     //Then
    //     }
    }
}