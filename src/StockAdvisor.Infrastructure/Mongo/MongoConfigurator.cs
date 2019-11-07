using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace StockAdvisor.Infrastructure.Mongo
{
    public static class MongoConfigurator
    {
        private static bool _initialized = false;

        public static void Initialize()
        {
            if(_initialized)
            {
                return;
            }
            RegisterConventions();
        }

        private static void RegisterConventions()
        {
            ConventionRegistry.Register("StockAdvisorConventions",
                                        new MongoConventions(),
                                        x => true);
            _initialized = true;
        }

        private class MongoConventions : IConventionPack
        {
            public IEnumerable<IConvention> Conventions => new List<IConvention>
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };
        }
    }
}