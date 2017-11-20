using System;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class DataInitialiser
    {
        public void FillSeriesWithDefaultNumbers(Series series, int minValue, int maxValue)
        {
            var random = new Random(unchecked((int)DateTime.Now.Ticks));

            for (var i = 0; i < series.Size; i++)
            {
                for (var j = 0; j < 12; j++)
                {
                    series.DataArray[i][j] = random.Next(minValue, maxValue);

                    if ((j + 1) % 3 == 0)
                    {
                        series.DataArray[i][j] = 0; //i row j column
                    }
                }
            }
        }
    }
}