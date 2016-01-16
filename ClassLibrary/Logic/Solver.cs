using System;
using System.Collections.Generic;
using System.Linq;

namespace cba.Logic
{
    public class Solver
    {
        public delegate bool Range(Series a);

        public int bestMedNum; //номер лучшего припарата

        public void Analise(int size, params Series[] table)
        {
            Range isInRange = IsInRange;
            bestMedNum = -1;
            var isGood = true;

            for (var i = 0; i < size; i++)
            {
                if (!IsChecked(table[i], isInRange))
                {
                    isGood = false;
                }
            }

            for (var i = size; i < 3; i++)
            {
                table[i].ratio = 0;
            }

            if (isGood)
            {
                bestMedNum = FindBestMed(table);
            }
        }

        public double FindRatio(Series a)
        {
            return (a.breeding[0] + a.breeding[1])/2;
        }

        public bool IsInRange(Series a)
        {
            if ((a.activity > 40000) && (a.activity < 60000))
            {
                return true;
            }
            return false;
        }

        public bool IsChecked(Series a, Range range)
        {
            if (range(a))
            {
                a.ratio = FindRatio(a);
                return true;
            }
            a.ratio = -1;
            return false;
        }

        public double FindMaxBreed(Series[] series)
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

        public double FindMinBreed(Series[] series)
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

        public int FindBestMed(params Series[] table)
        {
            double valueOfBest = 0;
            int numberOfBest;
            var l = new List<double>();

            foreach (var a in table)
            {
                l.Add(a.ratio);
            }

            valueOfBest = l.Max();
            numberOfBest = l.IndexOf(valueOfBest) + 1;
            Console.WriteLine();

            foreach (var a in table)
            {
                if (a.ratio == valueOfBest)
                {
                    Console.WriteLine(a.Name + " " + numberOfBest + " лучший");
                }
            }

            return numberOfBest - 1;
        }
    }
}