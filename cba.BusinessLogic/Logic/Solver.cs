using System;
using System.Linq;

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
                        if (series[k].a[i][j] < 0)
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
                    series[i].FindMidValues();
                    series[i].FindBreedingSum();
                    series[i].FindActivityValue();
                }

                var isSeriesValid = true;

                for (var i = 0; i < series[0].SizeOfSeries; i++)
                {
                    if (!IsChecked(series[i]))
                    {
                        isSeriesValid = false;
                    }
                }

                //for (var i = Series.Size; i < 3; i++)
                //{
                //    series[i].ratio = 0;
                //}

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
            return (a.breeding[0] + a.breeding[1])/2;
        }

        private bool IsInRange(Series a)
        {
            if ((a.activity > 40000) && (a.activity < 60000))
            {
                return true;
            }

            return false;
        }

        private bool IsChecked(Series a)
        {
            if (IsInRange(a))
            {
                a.ratio = FindRatio(a);
                return true;
            }

            a.ratio = -1;
            return false;
        }

        private double FindMaxBreed(Series[] series)
        {
            var maxBreed = series[0].breeding[0];

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    if (series[i].breeding[j] > maxBreed)
                    {
                        maxBreed = series[i].breeding[j];
                    }
                }
            }

            return maxBreed;
        }

        private double FindMinBreed(Series[] series)
        {
            var minBreed = series[0].breeding[0];

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    if (series[i].breeding[j] < minBreed)
                    {
                        minBreed = series[i].breeding[j];
                    }
                }
            }

            return minBreed;
        }

        private int FindBestMed(params Series[] series)
        {
            double valueOfBest = 0;

            var l = series.Select(a => a.ratio).ToList();

            valueOfBest = l.Max();

            var numberOfBest = l.IndexOf(valueOfBest) + 1;

            return numberOfBest;
        }
    }
}