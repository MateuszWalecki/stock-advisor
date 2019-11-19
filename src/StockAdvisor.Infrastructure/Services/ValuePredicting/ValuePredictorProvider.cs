using System.Collections.Generic;
using System.Linq;
using StockAdvisor.Infrastructure.Exceptions;
using StockAdvisor.Infrastructure.Extensions;

namespace StockAdvisor.Infrastructure.Services.ValuePredicting
{
    public class ValuePredictorProvider : IValuePredictorProvider
    {
        private readonly IEnumerable<IValuePredictor> _predictors;

        public ValuePredictorProvider(IEnumerable<IValuePredictor> predictors)
        {
            _predictors = predictors;
        }
        
        public IValuePredictor GetPredictor(string id)
        {
            if (id.Empty())
            {
                throw new WrongAlgorithmNameSerExc($"Algorithm name cannot be " +
                    $"null or white space");
            }

            var predictor = _predictors.SingleOrDefault(
                x => x.GetName().ToLowerInvariant() == id.ToLowerInvariant());

            if (predictor == null)
            {
                throw new WrongAlgorithmNameSerExc($"Algorithm name {id} " +
                    "is wrong.");
            }
            return predictor;
        }
    }
}