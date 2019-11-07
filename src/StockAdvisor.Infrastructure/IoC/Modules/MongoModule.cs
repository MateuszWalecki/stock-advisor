using System.Reflection;
using Autofac;
using MongoDB.Driver;
using StockAdvisor.Infrastructure.Mongo;
using StockAdvisor.Infrastructure.Repositories;

namespace StockAdvisor.Infrastructure.IoC.Modules
{
    public class MongoModule : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                var settings = c.Resolve<MongoSettings>();

                return new MongoClient(settings.ConnectionString);
            }).SingleInstance();
            
            builder.Register((c, p) =>
            {
                var client = c.Resolve<MongoClient>();
                var settings = c.Resolve<MongoSettings>();
                var database = client.GetDatabase(settings.Database);

                return database;
            }).As<IMongoDatabase>();

            var assembly = typeof(RepositoryModule)
                .GetTypeInfo()
                .Assembly;

            builder.RegisterAssemblyTypes(assembly)
                   .Where(x => x.IsAssignableTo<IMongoRepository>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}