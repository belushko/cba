using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class SeriesManager
    {
        public void FindBreedingSum(Series series)
        {
            series.Breeding[0] = 0;
            series.Breeding[1] = 0;
            series.Breeding[2] = 0;
            series.Breeding[3] = 0;

            for (var i = 0; i < series.Size; i++)
            {
                series.Breeding[0] += series.DataArray[i][2];
                series.Breeding[1] += series.DataArray[i][5];
                series.Breeding[2] += series.DataArray[i][8];
                series.Breeding[3] += series.DataArray[i][11];
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
                        series.DataArray[i][j] = (series.DataArray[i][j - 2] + series.DataArray[i][j - 1]) / 2;
                    }
                }
            }
        }

        public void FindActivityValue(Series series, double mOValue)
        {
            series.Activity =
                (series.Breeding[0] + series.Breeding[1])
                / (series.Breeding[2] + series.Breeding[3]) * mOValue;
        }
    }
}
