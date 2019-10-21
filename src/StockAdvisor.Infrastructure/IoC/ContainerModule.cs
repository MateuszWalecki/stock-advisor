using Autofac;
using Microsoft.Extensions.Configuration;
using StockAdvisor.Infrastructure.IoC.Modules;
using StockAdvisor.Infrastructure.Mappers;

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
            builder.RegisterInstance(AutoMapperConfig.Initailize())
                .SingleInstance();

            builder.RegisterModule(new RepositoryModule(_configuration));
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<CommandModule>();
            builder.RegisterModule(new SettingsModule(_configuration));
        }
    }
}