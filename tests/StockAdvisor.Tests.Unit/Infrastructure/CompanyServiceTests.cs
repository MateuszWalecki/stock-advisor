using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using StockAdvisor.Core.Domain;
using StockAdvisor.Core.Repositories;
using StockAdvisor.Infrastructure.DTO;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services;
using StockAdvisor.Infrastructure.Services.ValuePredicting;
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
            var predictorProviderMock = new Mock<IValuePredictorProvider>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

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

            var predictorProviderMock = new Mock<IValuePredictorProvider>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

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

            var predictorProviderMock = new Mock<IValuePredictorProvider>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

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

            var predictorProviderMock = new Mock<IValuePredictorProvider>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

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

            var predictorProviderMock = new Mock<IValuePredictorProvider>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

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

            var predictorProviderMock = new Mock<IValuePredictorProvider>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

        //When
            var returnedCollection = await comapnyService.GetValueStatusAsync(companySymbol);

        //Then
            returnedCollection.Should().BeSameAs(mappedCompanies);
        }

        [Fact]
        public async Task predict_values_throws_exception_if_company_repo_returns_null()
        {
        //Given
            string algorithm = "algorithm";

            var company = new Company("AAPL", "Apple Inc.", 50m);

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(x => x.GetCompanyValueStatusAsync(company.Name))
                           .ReturnsAsync((CompanyValueStatus)null);

            var mapperMock = new Mock<IMapper>();

            var predictorProviderMock = new Mock<IValuePredictorProvider>();

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

        //When
            Func<Task> act = () => comapnyService.PredictValues(company.Symbol, algorithm);

        //Then
            await Assert.ThrowsAsync<WrongCompanySymbolSerExc>(act);
            companyRepoMock.Verify(x => x.GetCompanyValueStatusAsync(company.Symbol),
                                   Times.Once);
        }

        [Fact]
        public async Task predict_values_calls_get_predictor_on_predictor_privder_and_predict_value_on_provider()
        {
        //Given
            string algorithm = "algorithm";

            var company = new Company("AAPL", "Apple Inc.", 50m);
            var companyValues = new List<CompanyValue>();
            var valueStatus = new CompanyValueStatus(company, companyValues);

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(x => x.GetCompanyValueStatusAsync(company.Symbol))
                           .ReturnsAsync(valueStatus);

            var mapperMock = new Mock<IMapper>();

            var predictorMock = new Mock<IValuePredictor>();
            var predictorProviderMock = new Mock<IValuePredictorProvider>();
            predictorProviderMock.Setup(x => x.GetPredictor(algorithm))
                                 .Returns(predictorMock.Object);

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

        //When
            await comapnyService.PredictValues(company.Symbol, algorithm);

        //Then
            companyRepoMock.Verify(x => x.GetCompanyValueStatusAsync(company.Symbol),
                                   Times.Once);
            predictorProviderMock.Verify(x => x.GetPredictor(algorithm), Times.Once);
            predictorMock.Verify(x => x.PredictValue(valueStatus), Times.Once);
        }

        [Fact]
        public async Task predict_values_calls_map_method_on_mapper_using_value_predictor_result()
        {
        //Given
            string algorithm = "algorithm";

            var company = new Company("AAPL", "Apple Inc.", 50m);
            var companyValues = new List<CompanyValue>();
            var historicalValueStatus = new CompanyValueStatus(company, companyValues);
            var predictedValueStatus = new CompanyValueStatus(company, companyValues);

            var companyRepoMock = new Mock<ICompanyRepository>();
            companyRepoMock.Setup(x => x.GetCompanyValueStatusAsync(company.Symbol))
                           .ReturnsAsync(historicalValueStatus);

            var mapperMock = new Mock<IMapper>();

            var predictorMock = new Mock<IValuePredictor>();
            predictorMock.Setup(x => x.PredictValue(historicalValueStatus))
                         .Returns(predictedValueStatus);

            var predictorProviderMock = new Mock<IValuePredictorProvider>();
            predictorProviderMock.Setup(x => x.GetPredictor(algorithm))
                                 .Returns(predictorMock.Object);

            var comapnyService = new CompanyService(companyRepoMock.Object,
                predictorProviderMock.Object, mapperMock.Object);

        //When
            await comapnyService.PredictValues(company.Symbol, algorithm);

        //Then
            mapperMock.Verify(x => x.Map<CompanyValueStatusDto>(predictedValueStatus),
                              Times.Once);
        }
    }
}