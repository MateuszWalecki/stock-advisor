using Microsoft.AspNetCore.Mvc.Testing;
using StockAdvisor.Api;

namespace StockAdvisor.Tests.EndToEnd.Configuration
{
    public static class WebAppFactoryConfigurator
    {
        public static WebApplicationFactory<Startup> ConfigureFactory(
            this WebApplicationFactory<Startup> factory)
        {
            return factory.SetPathToTestAppSettings();
        }

        private static WebApplicationFactory<Startup> SetPathToTestAppSettings(
            this WebApplicationFactory<Startup> factory)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context,conf) =>
                {
                    context.HostingEnvironment.EnvironmentName = "testing";
                });
            });
        }
    }
}