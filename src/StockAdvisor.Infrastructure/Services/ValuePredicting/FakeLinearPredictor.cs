using System;
using System.Linq;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.ValuePredicting
{
    public abstract class FakeLinearPredictor : IValuePredictor, IFakeValuePredictor
    {
        protected static readonly int ValuesToGenerate = 365 * 2;

        protected CompanyValueStatus Historical;
        protected TimeSpan TimeStep;
        protected decimal PriceStep;


        public abstract string GetName();

        public abstract CompanyValueStatus PredictValue(CompanyValueStatus historical);


        protected void CalculatePriceAndDateSteps()
        {
            TimeStep = Historical.HistoricalValue.Skip(1).First().Date -
                        Historical.HistoricalValue.First().Date;
            PriceStep =  Historical.HistoricalValue.Reverse().Skip(1).First().Price -
                          Historical.HistoricalValue.Reverse().First().Price;
        }

        protected bool PriceStepShouldBeReversed(decimal currentPrice)
            => currentPrice < 0;
    }
}