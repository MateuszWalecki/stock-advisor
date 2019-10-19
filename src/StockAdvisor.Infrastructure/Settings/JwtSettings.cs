namespace StockAdvisor.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public int TokenExpiryMinutes { get; set; }
        public string Issuer {get; set; }
    }
}