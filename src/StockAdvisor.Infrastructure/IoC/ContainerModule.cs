using Autofac;
using Microsoft.Extensions.Configuration;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.IoC.Modules;
using StockAdvisor.Infrastructure.Mappers;
using StockAdvisor.Infrastructure.Settings;

namespace StockAdvisor.Infrastructure.IoC
{
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }    

        protected override void Load(ContainerBuilder builder)
        {
            var generalSettings = _configuration.GetSettings<GeneralSettings>();

            builder.RegisterInstance(AutoMapperConfig.Initailize())
                .SingleInstance();

            builder.RegisterModule<RepositoryModule>();
            if (!generalSettings.InMemoryRepositories)
            {
                builder.RegisterModule<MongoModule>();
            }
            builder.RegisterModule(new ServiceModule(_configuration));
            builder.RegisterModule<CommandModule>();
            builder.RegisterModule(new SettingsModule(_configuration));
        }
    }
}