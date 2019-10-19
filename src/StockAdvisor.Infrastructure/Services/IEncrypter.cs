namespace StockAdvisor.Infrastructure.Services
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string password, string salt);
    }
}