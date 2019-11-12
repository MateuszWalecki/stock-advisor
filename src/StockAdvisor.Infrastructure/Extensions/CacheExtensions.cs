using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Extensions
{
    public static class CacheExtensions
    {
        public static void SetJwt(this IMemoryCache cache, Guid tokenId, JwtDto jwt)
            => cache.Set(GetJwtKey(tokenId), jwt, TimeSpan.FromSeconds(5));   

        public static JwtDto GetJwt(this IMemoryCache cache, Guid tokenId)
            => cache.Get<JwtDto>(GetJwtKey(tokenId));

        private static string GetJwtKey(Guid tokenId)
            => $"jwt-{tokenId}";


        public static void SetPredictedValues(this IMemoryCache cache, Guid valuesId,
            IEnumerable<CompanyValueStatusDto> predictedValues)
            => cache.Set(GetValuesKey(valuesId), predictedValues, TimeSpan.FromSeconds(5));   

        public static IEnumerable<CompanyValueStatusDto> GetPredictedValues(
            this IMemoryCache cache, Guid valuesId)
            => cache.Get<IEnumerable<CompanyValueStatusDto>>(GetValuesKey(valuesId));

        private static string GetValuesKey(Guid valuesId)
            => $"predicted-values-{valuesId}";
    }
}