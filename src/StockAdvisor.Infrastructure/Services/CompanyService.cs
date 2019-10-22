using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;

namespace StockAdvisor.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> BrowseAsync()
        {
            var companies = await _companyRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<Company>, IEnumerable<CompanyDto>>(companies);
        }

        public async Task<IEnumerable<HistoricalPriceDto>> GetHistoricalAsync(string companySymbol)
        {
            var company = await _companyRepository.GetAsync(companySymbol);

            return _mapper.Map<IEnumerable<HistoricalPrice>, IEnumerable<HistoricalPriceDto>>(company);
        }
    }
}