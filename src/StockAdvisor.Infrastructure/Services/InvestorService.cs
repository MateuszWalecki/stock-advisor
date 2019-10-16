using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Services
{
    public class InvestorService : IInvestorService
    {
        private readonly IInvestorRepository _investorRepository;

        protected InvestorService()
        {
        }

        public InvestorService(IInvestorRepository investorRepository)
        {
            _investorRepository = investorRepository;
        }

        public async Task<InvestorDto> GetAsync(Guid userId)
        {
            var investor = await _investorRepository.GetAsync(userId);

            if (investor == null)
            {
                throw new ServiceException(ErrorCodes.InvestorNotFound,
                    $"Investor related with user with id {userId} does not exist.");
            }

            return new InvestorDto()
            {
                UserId = investor.UserId,
                UpdatedAt = investor.UpdatedAt,
                FavouriteCompanies = new List<string>(investor.FavouriteCompanies)
            };
        }

        public async Task RegisterAsync(Guid userId)
        {
            var investor = await _investorRepository.GetAsync(userId);

            if (investor != null)
            {
                throw new ServiceException(ErrorCodes.InvestorExists,
                    $"Cannot register investor related with user with id {userId}, " +
                    "because it already exists.");
            }
            investor = new Investor(userId);
            await _investorRepository.AddAsync(investor);
        }
    }
}