using System;
using System.Linq;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class Solver
    {
        public int bestMedNum; //номер лучшего припарата

        public int Culculate(Series[] series)
        {
            var isTableValid = true;

            for (var k = 0; k < series[0].SizeOfSeries; k++)
            {
                for (var i = 0; i < series[0].Size; i++)
                {
                    for (var j = 0; j < 12; j++)
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

                    seriesManager.FindMidValues(series[i]);
                    seriesManager.FindBreedingSum(series[i]);
                    seriesManager.FindActivityValue(series[i]);
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
                    bestMedNum = FindBestMed(series);
                }

                return bestMedNum;
            }

            throw new Exception("Не все препараты прошли проверку!");
        }

        private double FindRatio(Series a)
        {
            return (a.Breeding[0] + a.Breeding[1])/2;
        }

        private bool IsInRange(Series a)
        {
            if ((a.Activity > 40000) && (a.Activity < 60000))
            {
                return true;
            }

            return false;
        }

        private bool IsChecked(Series a)
        {
            if (IsInRange(a))
            {
                a.Ratio = FindRatio(a);
                return true;
            }

            a.Ratio = -1;

            return false;
        }

        private int FindBestMed(params Series[] series)
        {
            double valueOfBest = 0;
            var l = series.Select(a => a.Ratio).ToList();
            valueOfBest = l.Max();
            var numberOfBest = l.IndexOf(valueOfBest) + 1;

            return numberOfBest;
        }
    }
}