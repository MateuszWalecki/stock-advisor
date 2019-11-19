namespace StockAdvisor.Infrastructure.Services
{
    public interface IEncrypter
    {
        string GetSalt();
#nullable enable
        string? GetHash(string password, string salt);
#nullable disable
    }
}