using Autofac;
using Microsoft.Extensions.Configuration;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.Settings;

namespace StockAdvisor.Infrastructure.IoC.Modules
{
    public class SettingsModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;
        
        public SettingsModule(IConfiguration configuration)
        {
            _configuration = configuration;    
        }

        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterInstance(_configuration.GetSettings<GeneralSettings>())
                   .SingleInstance();
        }
    }
}