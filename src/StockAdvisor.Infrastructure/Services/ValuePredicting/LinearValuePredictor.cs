using System.Collections.Generic;
using System.Linq;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.ValuePredicting
{
    public class LinearValuePredictor : FakeLinearPredictor
    {
        public override string GetName()
            => "Linear";

        public override CompanyValueStatus PredictValue(CompanyValueStatus historical)
        {
            Historical = historical;

            CalculatePriceAndDateSteps();
            var generatedValues = GenerateCollectionToReturn();

            return new CompanyValueStatus(historical.Company, generatedValues);
        }

        private IEnumerable<CompanyValue> GenerateCollectionToReturn()
        {
            var currentDate = Historical.HistoricalValue.Last().Date;
            var currentPrice = Historical.HistoricalValue.Last().Price;

            var valuesToExtend = new List<CompanyValue>(Historical.HistoricalValue);

            for (int i = 0; i < ValuesToGenerate; i++)
            {
                currentDate += TimeStep;
                currentPrice += PriceStep;
                
                if (PriceStepShouldBeReversed(currentPrice))
                {
                    PriceStep *= -1;
                    currentPrice += 2 * PriceStep;
                }

                var toAdd = new CompanyValue(currentDate, currentPrice);
                valuesToExtend.Add(toAdd);
            }

            return valuesToExtend;
        }
    }
}