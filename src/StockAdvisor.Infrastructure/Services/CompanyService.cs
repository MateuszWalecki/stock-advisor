using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services.ValuePredicting;

namespace StockAdvisor.Infrastructure.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IValuePredictorProvider _predictorProvider;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository,
            IValuePredictorProvider predictorProvider, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _predictorProvider = predictorProvider;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> BrowseAsync()
        {
            var companies = await _companyRepository.BrowseAsync();

            return _mapper.Map<IEnumerable<CompanyDto>>(companies);
        }

        public async Task<CompanyValueStatusDto> GetValueStatusAsync(string companySymbol)
        {
            var company = await _companyRepository.GetCompanyValueStatusAsync(companySymbol);

            return _mapper.Map<CompanyValueStatusDto>(company);
        }

        public async Task<CompanyValueStatusDto> PredictValues(string companySymbol,
            string algorithm)
        {
            var companyValueStatus = await _companyRepository.GetCompanyValueStatusAsync(
                companySymbol);

            if (companyValueStatus == null)
            {
                throw new WrongCompanySymbolSerExc($"Company symbol {companySymbol} " +
                    "is wrong.");
            }

            var valuePredictor = _predictorProvider.GetPredictor(algorithm);

            var predictedValues = valuePredictor.PredictValue(companyValueStatus);

            return _mapper.Map<CompanyValueStatusDto>(predictedValues);
        }
    }
}