using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.Settings;

namespace StockAdvisor.Infrastructure.IoC.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;
        
        public RepositoryModule(IConfiguration configuration)
        {
            _configuration = configuration;    
        }

        protected override void Load(Autofac.ContainerBuilder builder)
        {
            var assembly = typeof(RepositoryModule)
                .GetTypeInfo()
                .Assembly;

            var settings = _configuration.GetSettings<GeneralSettings>();

            if (settings.InMemoryRepositories)
            {
                //singleton to single initialize repository with data and to hold it their 
                builder.RegisterAssemblyTypes(assembly)
                       .Where(x => x.IsAssignableTo<IRepository>())
                       .AsImplementedInterfaces()
                       .SingleInstance();
            }
            else
            {
                builder.RegisterAssemblyTypes(assembly)
                       .Where(x => x.IsAssignableTo<IRepository>())
                       .AsImplementedInterfaces()
                       .InstancePerLifetimeScope();
            }
        }
    }
}