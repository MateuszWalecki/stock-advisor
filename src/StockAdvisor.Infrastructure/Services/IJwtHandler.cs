using System;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public interface IJwtHandler
    {
        JwtDto CreateToken(Guid userId, string role);
    }
}