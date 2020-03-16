using StockAdvisor.Core.Domain;
using System;

namespace StockAdvisor.Infrastructure.Services.DataInitializer
{
    /// <summary>
    /// Allows to operate on user abstraction with access to its password, that is imposible in case of
    /// standard user (not in testing) for obvious reasons.
    /// </summary>
    public class UserWrapperForTesting : User
    {
        public string Password { get; protected set; }

        public UserWrapperForTesting(Guid userId, string emailAddress, string firstName,
             string surName, string password, UserRole userRole) :
            base(userId, emailAddress, firstName,
             surName, "hash", "salt", userRole)
        {
            Password = password;
        }
    }
}
