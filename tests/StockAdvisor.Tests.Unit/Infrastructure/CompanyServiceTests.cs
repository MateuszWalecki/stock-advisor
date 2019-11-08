using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Services;
using Xunit;

namespace StockAdvisor.Tests.Unit.Infrastructure
{
    public class CompanyServiceTests
    {
        [Fact]
        public async Task browse_asyc_calls_browse_async_on_copmanies_repo()
        {
        //Given
            var companyRepoMock = new Mock<ICompanyRepository>();
            var mapperMock = new Mock<IMapper>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                mapperMock.Object);

        //When
            await comapnyService.BrowseAsync();

        //Then
            companyRepoMock.Verify(x => x.BrowseAsync(), Times.Once);
        }

        [Fact]
        public async Task browse_asyc_calls_mappers_map_method_using_repos_browse_async_result()
        {
        //Given
            IEnumerable<Company> companiesFromRepo = new List<Company>();
            IEnumerable<Company> companiesGivenToMap = null;

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(x => x.BrowseAsync())
                           .ReturnsAsync(companiesFromRepo);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<CompanyDto>>(companiesFromRepo))
                      .Callback<object>(x => companiesGivenToMap = x as IEnumerable<Company>);

            var comapnyService = new CompanyService(companyRepoMock.Object,
                mapperMock.Object);

        //When
            await comapnyService.BrowseAsync();

        //Then
            mapperMock.Verify(x => x.Map<IEnumerable<CompanyDto>>(companiesFromRepo),
                              Times.Once);
            companiesFromRepo.Should().BeSameAs(companiesGivenToMap);
        }

        [Fact]
        public async Task browse_asyc_returns_mappers_map_result()
        {
        //Given
            IEnumerable<CompanyDto> mappedCompanies = new List<CompanyDto>();

            var companyRepoMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<CompanyDto>>(
                            It.IsAny<IEnumerable<Company>>()))
                      .Returns(mappedCompanies);

            var comapnyService = new CompanyService(companyRepoMock.Object,
                mapperMock.Object);

        //When
            var returnedCollection = await comapnyService.BrowseAsync();

        //Then
            returnedCollection.Should().BeSameAs(mappedCompanies);
        }

        [Fact]
        public async Task get_value_status_asyc_calls_get_async_on_copmanies_repo()
        {
        //Given
            string companySymbol = "AAPL";
            var companyRepoMock = new Mock<ICompanyRepository>();
            var mapperMock = new Mock<IMapper>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                mapperMock.Object);

        //When
            await comapnyService.GetValueStatusAsync(companySymbol);

        //Then
            companyRepoMock.Verify(x => x.GetCompanyValueStatusAsync(companySymbol), Times.Once);
        }

        [Fact]
        public async Task get_value_status_asyc_calls_mappers_map_method_using_repos_get_async_result()
        {
        //Given
            string companySymbol = "AAPL";
            var historicalFromRepo = new Mock<CompanyValueStatus>();
            CompanyValueStatus givenToMap = null;

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(x => x.GetCompanyValueStatusAsync(companySymbol))
                           .ReturnsAsync(historicalFromRepo.Object);

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<CompanyValueStatusDto>(
                                historicalFromRepo.Object))
                      .Callback<object>(x => givenToMap = x as CompanyValueStatus);

            var comapnyService = new CompanyService(companyRepoMock.Object,
                mapperMock.Object);

        //When
            await comapnyService.GetValueStatusAsync(companySymbol);

        //Then
            mapperMock.Verify(x => x.Map<CompanyValueStatusDto>(
                                    historicalFromRepo.Object),
                              Times.Once);
            historicalFromRepo.Object.Should().BeSameAs(givenToMap);
        }

        [Fact]
        public async Task get_value_status_asyc_returns_mappers_map_result()
        {
        //Given
            string companySymbol = "AAPL";

            CompanyValueStatusDto mappedCompanies = new CompanyValueStatusDto();

            var companyRepoMock = new Mock<ICompanyRepository>();

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<CompanyValueStatusDto>(
                            It.IsAny<CompanyValueStatus>()))
                      .Returns(mappedCompanies);

            var comapnyService = new CompanyService(companyRepoMock.Object,
                mapperMock.Object);

        //When
            var returnedCollection = await comapnyService.GetValueStatusAsync(companySymbol);

        //Then
            returnedCollection.Should().BeSameAs(mappedCompanies);
        }
    }
}