using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.Repositories;
using StockAdvisor.Infrastructure.Settings;
using Xunit;

namespace StockAdvisor.UnitTests.Infrastructure
{
    public class FinancialModelingPrepRepositoryTests
    {
        [Fact]
        public async Task browse_async_checks_if_cache_contains_proper_data()
        {
            //Given
            var cacheResult = new List<Company>();

            var httpClientMock = new Mock<HttpClient>();

            var memoryCacheMock = 
                MockMemoryCacheService.RegisterMemoryCache(cacheResult);

            var sut = new FinancialModelingPrepRepository(httpClientMock.Object,
                memoryCacheMock.Object);

            //When
            var result = await sut.BrowseAsync();

            //Then
            MockMemoryCacheService.GetMethodWasCalledOnce(memoryCacheMock);
            result.Should().BeSameAs(cacheResult);
        }

        [Fact]
        public async Task browse_async_calls_httpclient_with_defined_uri_if_cache_get_returns_null()
        {
            //Given
            var expectedUri = new Uri("https://financialmodelingprep.com/api/v3/company/stock/list");

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       // Setup the PROTECTED method to mock
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>()
                       )
                       // prepare the expected response of the mocked http call
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent("{\n  \"symbolsList\" : [ {\n    \"symbol\" : \"SPY\",\n    \"name\" : \"SPDR S&P 500\",\n    \"price\" : 299.39\n  }, {\n    \"symbol\" : \"CMCSA\",\n    \"name\" : \"Comcast Corporation Class A Common Stock\",\n    \"price\" : 45.54\n  } ]\n}")
                       })
                       .Verifiable();

            // use real http client with mocked handler here
            var httpClinet = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://financialmodelingprep.com")
            };

            var memoryCacheMock = MockMemoryCacheService.RegisterMemoryCache(null);

            var sut = new FinancialModelingPrepRepository(httpClinet,
                memoryCacheMock.Object);

            //When
            var result = await sut.BrowseAsync();

            //Then
            result.Should().NotBeNull();
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get  // we expected a GET request
                    && req.RequestUri == expectedUri // to this uri
                ),
                ItExpr.IsAny<CancellationToken>()
                );
            MockMemoryCacheService.GetMethodWasCalledOnce(memoryCacheMock);            
        }
        
        [Fact]
        public async Task browse_async_saves_http_request_result_to_cache_if_it_returns_null()
        {
            //Given
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       // Setup the PROTECTED method to mock
                       .Setup<Task<HttpResponseMessage>>(
                           "SendAsync",
                           ItExpr.IsAny<HttpRequestMessage>(),
                           ItExpr.IsAny<CancellationToken>()
                       )
                       // prepare the expected response of the mocked http call
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent("{\n  \"symbolsList\" : [ {\n    \"symbol\" : \"SPY\",\n    \"name\" : \"SPDR S&P 500\",\n    \"price\" : 299.39\n  }, {\n    \"symbol\" : \"CMCSA\",\n    \"name\" : \"Comcast Corporation Class A Common Stock\",\n    \"price\" : 45.54\n  } ]\n}")
                       })
                       .Verifiable();

            // use real http client with mocked handler here
            var httpClinet = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://financialmodelingprep.com")
            };

            var memoryCacheMock = MockMemoryCacheService.RegisterMemoryCache(null);

            var sut = new FinancialModelingPrepRepository(httpClinet,
                memoryCacheMock.Object);

            //When
            await sut.BrowseAsync();

            //Then
            MockMemoryCacheService.SetMethodWasCalledOnce(memoryCacheMock);
        }

        [Fact]
        public async Task browse_async_does_not_call_httpclient_if_cache_get_method_returns_not_null()
        {
            //Given
            var cacheResult = new List<Company>();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClinet = new HttpClient(handlerMock.Object);

            var memoryCacheMock = MockMemoryCacheService.RegisterMemoryCache(cacheResult);

            var sut = new FinancialModelingPrepRepository(httpClinet,
                memoryCacheMock.Object);

            //When
            var result = await sut.BrowseAsync();

            //Then
            result.Should().BeSameAs(cacheResult);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(0), // we expect that http client has not been used
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        // used methods of the IMemoryCache are extension methods and cannot 
        // setup their directly, so need to setup methods called indirectly
        public static class MockMemoryCacheService
        {
            public static Mock<IMemoryCache> RegisterMemoryCache(object expectedValue)
            {
                var mockMemoryCache = new Mock<IMemoryCache>();
                mockMemoryCache
                    .Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                    .Returns(true);

                var cacheEntryMock = new Mock<ICacheEntry>();

                mockMemoryCache
                    .Setup(x => x.CreateEntry(It.IsAny<object>()))
                    .Returns(cacheEntryMock.Object);

                return mockMemoryCache;
            }

            public static void GetMethodWasCalledOnce(Mock<IMemoryCache> mock)
            {
                object expectedVal;
                mock.Verify(x => x.TryGetValue(It.IsAny<object>(), out expectedVal),
                            Times.Once);
            }

            public static void SetMethodWasCalledOnce(Mock<IMemoryCache> mock)
            {
                mock.Verify(x => x.CreateEntry(It.IsAny<string>()),
                            Times.Once);
            }
        }
    }
}