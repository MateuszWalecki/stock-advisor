namespace StockAdvisor.Infrastructure.Settings
{
    public class GeneralSettings
    {
        public string Name { get; set; }
        public bool InMemoryRepositories { get; set; }
        public bool SeedData { get; set; }
        public bool MongoDatabase { get; set; }
        public GeneralSettings()
        {
        }
    }
}