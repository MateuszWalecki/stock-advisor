using System;
using System.Collections.Generic;
using System.Linq;
using StockAdvisor.Core.Domain;

namespace StockAdvisor.Infrastructure.Services.ValuePredicting
{
    public class NoisedLinearValuePredictor : IValuePredictor, IFakeValuePredictor
    {
        private static readonly int _valuesToGenerate = 365 * 2;

        private readonly Random _noiseGenerator = new Random();

        public string GetName()
            => "Noised-Linear";

        public CompanyValueStatus PredictValue(CompanyValueStatus historical)
        {
            TimeSpan timeStep = historical.HistoricalValue.Skip(1).First().Date -
                                historical.HistoricalValue.First().Date;
            decimal priceStep =  historical.HistoricalValue.First().Price -
                                 historical.HistoricalValue.Skip(1).First().Price;

            var currentDate = historical.HistoricalValue.Last().Date;
            var currentPrice = historical.HistoricalValue.Last().Price;

            var valuesToExtend = new List<CompanyValue>(historical.HistoricalValue);

            for (int i = 0; i < _valuesToGenerate; i++)
            {
                priceStep += _noiseGenerator.Next(-100, 100) / 100.0m * priceStep;

                currentDate += timeStep;
                currentPrice += priceStep;
                
                if (currentPrice < 0)
                {
                    priceStep *= -1;
                    currentPrice += 2 * priceStep;
                }

                var toAdd = new CompanyValue(currentDate, currentPrice);
                valuesToExtend.Add(toAdd);
            }

            return new CompanyValueStatus(historical.Company, valuesToExtend);
        }
    }
}