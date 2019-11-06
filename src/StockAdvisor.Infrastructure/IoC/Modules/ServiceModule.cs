using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Services.DataInitializer;
using StockAdvisor.Infrastructure.Settings;

namespace StockAdvisor.Infrastructure.IoC.Modules
{
    public class ServiceModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;
        
        public ServiceModule(IConfiguration configuration)
        {
            _configuration = configuration;    
        }
        
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            var assembly = typeof(ServiceModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IService>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<Encrypter>()
                    .As<IEncrypter>()
                    .SingleInstance();

            builder.RegisterType<JwtHandler>()
                    .As<IJwtHandler>()
                    .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IInputDataBuilder>())
                   .SingleInstance();
            
            var financialSettings = _configuration.GetSettings<FinancialModelingPrepSettings>();
            builder.Register(x => new HttpClient()
                    {
                        BaseAddress = new Uri(financialSettings.Uri)
                    })
                    .SingleInstance();
        }
    }
}