using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using StockAdvisor.Core.Domain;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Services.ValuePredicting;
using Xunit;

namespace StockAdvisor.Tests.Unit.Infrastructure
{
    public class ValuePredictorProviderTests
    {
        [Fact]
        public void get_predictor_throws_exception_if_given_collection_in_constructor_does_not_return_single_or_default_on_filtering_by_given_id()
        {
        //Given
            string id = "id";

            IEnumerable<IValuePredictor> predictors = new List<IValuePredictor>
            {
                new ValuePredictorMock(id),
                new ValuePredictorMock(id)
            };

            var provider = new ValuePredictorProvider(predictors);

        //When
            Action act = () => provider.GetPredictor(id);

        //Then
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void get_predictor_throws_exception_if_given_collection_in_constructor_does_not_contain_any_matching_predictor()
        {
        //Given
            string id = "id";
            string otherId = "other";

            IEnumerable<IValuePredictor> predictors = new List<IValuePredictor>
            {
                new ValuePredictorMock(id),
            };

            var provider = new ValuePredictorProvider(predictors);

        //When
            Action act = () => provider.GetPredictor(otherId);

        //Then
            Assert.Throws<WrongAlgorithmNameSerExc>(act);
        }

        [Fact]
        public void get_predictor_returns_matching_predictor_if_exists_in_given_collection()
        {
        //Given
            string id = "id";
            string otherId = "other";
            var matchingPredictor = new ValuePredictorMock(id);

            IEnumerable<IValuePredictor> predictors = new List<IValuePredictor>
            {
                matchingPredictor,
                new ValuePredictorMock(otherId)
            };

            var provider = new ValuePredictorProvider(predictors);

        //When
            var result = provider.GetPredictor(id);

        //Then
            result.Should().BeSameAs(matchingPredictor);
        }

        private class ValuePredictorMock : IValuePredictor
        {
            private readonly string _id;

            public ValuePredictorMock(string id)
            {
                _id = id;
            }

            public string GetName() => _id;

            public CompanyValueStatus PredictValue(CompanyValueStatus historical)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}