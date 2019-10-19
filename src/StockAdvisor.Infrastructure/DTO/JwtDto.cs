using System;
using System.Collections.Generic;

namespace StockAdvisor.Infrastructure.DTO
{
    public class JwtDto
    {
        public string Token { get; set; }
        public long ExpiryTicks { get; set; }
    }
}