using System;
using System.Linq;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class Solver
    {
        public int bestDrugNumber = 1;
        
        public int Culculate(Series[] series)
        {
            if (!IsTableValid(series))
                throw new Exception("Не все препараты прошли проверку!");

            for (var i = 0; i < series[0].SizeOfSeries; i++)
            {
                FindInnerValues(series[i]);
            }

            if (IsSeriesValid(series))
            {
                bestDrugNumber = FindBestDrugNumber(series);
            }

            return bestDrugNumber;
        }

        private void FindInnerValues(Series series)
        {
            var seriesManager = new SeriesManager();
            var mOValue = series.MOValue;

            seriesManager.FindMidValues(series);
            seriesManager.FindBreedingSum(series);
            seriesManager.FindActivityValue(series, mOValue);
        }

        private bool IsSeriesValid(Series[] series)
        {
            for (var i = 0; i < series[0].SizeOfSeries; i++)
            {
                if (!IsChecked(series[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsTableValid(Series[] series)
        {
            const int numberOfColumns = 12;

            for (var k = 0; k < series[0].SizeOfSeries; k++)
            {
                for (var i = 0; i < series[0].Size; i++)
                {
                    for (var j = 0; j < numberOfColumns; j++)
                    {
                        if (series[k].DataArray[i][j] < 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private double FindRatio(Series series)
        {
            return (series.Breeding[0] + series.Breeding[1]) / 2;
        }

        private bool IsSeriesInRange(Series series)
        {
            if ((40000 < series.Activity) && (series.Activity < 60000))
            {
                return true;
            }

            return false;
        }

        private bool IsChecked(Series series)
        {
            if (IsSeriesInRange(series))
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