using Microsoft.AspNetCore.Mvc.Testing;
using StockAdvisor.Api;

namespace StockAdvisor.Tests.EndToEnd.Configuration
{
    public static class WebAppFactoryConfigurator
    {
        public static WebApplicationFactory<Startup> ConfigureFactory(
            this WebApplicationFactory<Startup> factory)
        {
            return factory.SetEnvironmentVariable();
        }

        private static WebApplicationFactory<Startup> SetEnvironmentVariable(
            this WebApplicationFactory<Startup> factory)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context,conf) =>
                {
                    context.HostingEnvironment.EnvironmentName = "Testing";
                });
            });
        }
    }
}