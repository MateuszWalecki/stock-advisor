using System.Collections.Generic;
using System.Linq;
using StockAdvisor.Infrastructure.Exceptions;

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