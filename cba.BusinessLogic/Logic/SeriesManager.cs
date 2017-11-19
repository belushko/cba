using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class SeriesManager
    {
        public void FindBreedingSum(Series series)
        {
            series.breeding[0] = 0;
            series.breeding[1] = 0;
            series.breeding[2] = 0;
            series.breeding[3] = 0;

            for (var i = 0; i < series.Size; i++)
            {
                series.breeding[0] += series.a[i][2];
                series.breeding[1] += series.a[i][5];
                series.breeding[2] += series.a[i][8];
                series.breeding[3] += series.a[i][11];
            }
        }

        public void FindMidValues(Series series)
        {
            for (var i = 0; i < series.Size; i++)
            {
                for (var j = 0; j < 12; j++)
                {
                    if ((j + 1) % 3 == 0)
                    {
                        series.a[i][j] = (series.a[i][j - 2] + series.a[i][j - 1]) / 2;
                    }
                }
            }
        }

        public void FindActivityValue(Series series)
        {
            series.activity =
                (series.breeding[0] + series.breeding[1])
                / (series.breeding[2] + series.breeding[3])
                * series.MOValue;
        }
    }
}
