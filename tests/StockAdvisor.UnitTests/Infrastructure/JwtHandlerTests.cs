using System;
using FluentAssertions;
using StockAdvisor.Infrastructure.Extensions;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Settings;
using Xunit;

namespace StockAdvisor.UnitTests.Infrastructure
{
    public class JwtHandlerTests
    {
        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        public void creating_token_expiry_date_is_given_in_settings(int expiryMinutes)
        {
            //Given
            var now = DateTime.UtcNow;
            string email = "test@email.com",
                role = "User";
            var lessThanExpiryMinutes = now.AddMinutes(expiryMinutes - 1).ToTimeStamp();
            var moreThanExpiryMinutes = now.AddMinutes(expiryMinutes + 1).ToTimeStamp();

            var settings= new JwtSettings()
            {
                TokenExpiryMinutes = expiryMinutes,
                Issuer = "https://localhost:5001",
                Key = "!SuperSecretKey!"
            };

            var jwtHandler = new JwtHandler(settings);

            //When
            var jwt = jwtHandler.CreateToken(email, role);

            //Then
            jwt.Should().NotBeNull();
            jwt.Token.Should().NotBeNull();
            jwt.ExpiryTicks.Should().BeGreaterThan(lessThanExpiryMinutes);
            jwt.ExpiryTicks.Should().BeLessThan(moreThanExpiryMinutes);
        }
    }
}