using System;
using Microsoft.Extensions.Caching.Memory;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Extensions
{
    public static class CacheExtensions
    {
        public static void SetJwt(this IMemoryCache cache, Guid tokenId, JwtDto jwt)
            => cache.Set(tokenId, jwt, TimeSpan.FromSeconds(5));   

        public static JwtDto GetJwt(this IMemoryCache cache, Guid tokenIn)
            => cache.Get<JwtDto>(tokenIn);

        private static string GetJwtKey(Guid tokenId)
            => $"jwt-{tokenId}";
    }
}