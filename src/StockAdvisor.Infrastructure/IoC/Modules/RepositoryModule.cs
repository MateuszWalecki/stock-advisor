using System.Reflection;
using Autofac;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Repositories;
using StockAdvisor.Infrastructure.Repositories.FakeDatabases;

namespace StockAdvisor.Infrastructure.IoC.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            var assembly = typeof(RepositoryModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IFakeDatabase>())
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IRepository>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<InMemoryRepository>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}