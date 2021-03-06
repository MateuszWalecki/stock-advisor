using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAdvisor.Infrastructure.Commands;
using StockAdvisor.Infrastructure.Services;

namespace StockAdvisor.Api.Controllers
{
    public class CompaniesController : ApiControllerBase
    {
        private readonly ICompanyService _companyService;
        
        public CompaniesController(ICommandDispatcher commandDispatcher,
            ICompanyService companyService) : base(commandDispatcher)
        {
            _companyService = companyService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await _companyService.BrowseAsync();

            if (companies == null || !companies.Any())
            {
                return NotFound("Cannot found any company.");
            }

            return Ok(companies);
        }
        
        [Authorize]
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetCompany(string symbol)
        {
            var company = await _companyService.GetValueStatusAsync(symbol);

            if (company == null)
            {
                return NotFound($"Cannot found any company with symbol {symbol}.");
            }

            return Ok(company);
        }

        [Authorize]
        [HttpGet("predict")]
        public async Task<IActionResult> PredictValues(
            [FromQuery]string companySymbol,
            [FromQuery]string algorithm)
        {
            var result = await _companyService.PredictValues(companySymbol, algorithm);

            return Ok(result);
        }
    }
}