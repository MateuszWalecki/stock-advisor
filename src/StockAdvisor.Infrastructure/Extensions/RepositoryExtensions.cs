using System;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<User> GetUserOrFailAsync(
            this IUserRepository repository, Guid userId)
        {
            var user = await repository.GetAsync(userId);
            
            if(user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound, 
                    $"User with id: '{userId}' was not found.");
            }

            return user;            
        }

        public static async Task<Investor> GetInvestorOrFailAsync(
            this IInvestorRepository repository, Guid userId)
        {
            var investor = await repository.GetAsync(userId);
            if(investor == null)
            {
                throw new ServiceException(ErrorCodes.InvestorNotFound, 
                    $"Investor with user id: '{userId}' was not found.");
            }

            return investor;            
        }
    }
}