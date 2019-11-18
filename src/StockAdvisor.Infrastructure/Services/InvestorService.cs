using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Extensions;

namespace StockAdvisor.Infrastructure.Services
{
    public class InvestorService : IInvestorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IInvestorRepository _investorRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        protected InvestorService()
        {
        }

        public InvestorService(IUserRepository userRepository,
            IInvestorRepository investorRepository, 
            ICompanyRepository companyRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _investorRepository = investorRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<InvestorDto> GetAsync(Guid userId)
        {
            var investor = await _investorRepository.GetAsync(userId);

            return _mapper.Map<InvestorDto>(investor);
        }

        public async Task<InvestorDto> GetAsync(string email)
        {
            var user = await _userRepository.GetAsync(email);
            
            if (user == null)
            {
                return null;
            }

            var investor = await _investorRepository.GetAsync(user.Id);

            return _mapper.Map<InvestorDto>(investor);
        }

        public async Task<IEnumerable<InvestorDto>> BrowseAsync()
        {
            var investors = await _investorRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<InvestorDto>>(investors);
        }

        public async Task RegisterAsync(Guid userId)
        {
            var user = await _userRepository.GetUserOrFailAsync(userId);

            var investor = await _investorRepository.GetAsync(userId);
            if (investor != null)
            {
                throw new InvestorExistsSerExc(
                    $"Investor with user id {userId} already exists.");
            }
            
            investor = new Investor(user);
            await _investorRepository.AddAsync(investor);
        }

        public async Task AddToFavouriteCompaniesAsync(Guid userId, string companySymbol)
        {
            var investor = await _investorRepository.GetInvestorOrFailAsync(userId);

            if (investor.FavouriteCompanies.Contains(companySymbol))
            {
                throw new CompanySymbolInUseSerExc($"Company symbol {companySymbol} currently " +
                    "exists in favourites collection.");
            }

            var allCompanies = await _companyRepository.BrowseAsync();
            if (!allCompanies.Where(x => x.Symbol == companySymbol).Any())
            {
                throw new InvalidCompanySymbolSerExc($"Given company symbol {companySymbol} " +
                    "is incorrect.");
            }

            investor.AddToFavouriteCompanies(companySymbol);

            await _investorRepository.UpdateAsync(investor);
        }

        public async Task RemoveFromFavouriteCompaniesAsync(Guid userId, string companySymbol)
        {
            var investor = await _investorRepository.GetInvestorOrFailAsync(userId);
            
            if (!investor.FavouriteCompanies.Contains(companySymbol))
            {
                throw new WrongCompanySymbolSerExc($"Company symbol {companySymbol} " +
                    "does not exist in favourties collection.");
            }

            investor.RemoveFromFavouriteCompanies(companySymbol);
            
            await _investorRepository.UpdateAsync(investor);
        }

        public async Task RemoveAsync(Guid userId)
        {
            await _investorRepository.GetInvestorOrFailAsync(userId);

            await _investorRepository.RemoveAsync(userId);
        }
    }
}