using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Services
{
    public class InvestorService : IInvestorService
    {
        private readonly IInvestorRepository _investorRepository;
        private readonly IMapper _mapper;

        protected InvestorService()
        {
        }

        public InvestorService(IInvestorRepository investorRepository, IMapper mapper)
        {
            _investorRepository = investorRepository;
            _mapper = mapper;
        }

        public async Task<InvestorDto> GetAsync(Guid userId)
        {
            var investor = await _investorRepository.GetAsync(userId);

            return _mapper.Map<Investor, InvestorDto>(investor);
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