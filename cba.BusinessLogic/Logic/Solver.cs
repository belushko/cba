using System;
using System.Linq;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class Solver
    {
        public int bestDrugNumber = 1; //номер лучшего припарата

        public int Culculate(Series[] series)
        {
            var isTableValid = true;

            var numberOfColumns = 12;

            for (var k = 0; k < series[0].SizeOfSeries; k++)
            {
                for (var i = 0; i < series[0].Size; i++)
                {
                    for (var j = 0; j < numberOfColumns; j++)
                    {
                        if (series[k].DataArray[i][j] < 0)
                        {
                            isTableValid = false;
                        }
                    }
                }
            }

            if (isTableValid)
            {
                for (var i = 0; i < series[0].SizeOfSeries; i++)
                {
                    var seriesManager = new SeriesManager();
                    var mOValue = series[0].MOValue;

                    seriesManager.FindMidValues(series[i]);
                    seriesManager.FindBreedingSum(series[i]);
                    seriesManager.FindActivityValue(series[i], mOValue);
                }

                var isSeriesValid = true;

                for (var i = 0; i < series[0].SizeOfSeries; i++)
                {
                    if (!IsChecked(series[i]))
                    {
                        isSeriesValid = false;
                    }
                }

                if (isSeriesValid)
                {
                    bestDrugNumber = FindBestDrugNumber(series);
                }

                return bestDrugNumber;
            }

            throw new Exception("Не все препараты прошли проверку!");
        }

        private double FindRatio(Series series)
        {
            return (series.Breeding[0] + series.Breeding[1]) / 2;
        }

        private bool IsInRange(Series series)
        {
            if ((40000 < series.Activity) && (series.Activity < 60000))
            {
                return true;
            }

            return false;
        }

        private bool IsChecked(Series series)
        {
            if (IsInRange(series))
            {
                series.Ratio = FindRatio(series);

                return true;
            }

            series.Ratio = -1;

            return false;
        }

        //TODO: remake method
        private int FindBestDrugNumber(params Series[] series)
        {
            double valueOfBest = 0;

            var ratioList = series.Select(serie => serie.Ratio).ToList();

            valueOfBest = ratioList.Max();

            var numberOfBest = ratioList.IndexOf(valueOfBest) + 1;

            return numberOfBest;
        }
    }
}