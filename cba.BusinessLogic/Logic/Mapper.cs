using System.Collections.Generic;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class Mapper
    {
        public List<double[]> DataArrayToItems(Series series)
        {
            var resultSeries = new Series
            {
                Items = new List<double[]>()
            };

            for (var i = 0; i < series.Size; i++)
            {
                var data = new double[12];

                for (var j = 0; j < 12; j++)
                {
                    data[j] = series.DataArray[i][j];
                }

                resultSeries.Items.Add(data);
            }

            return resultSeries.Items;
        }

        public DataArray ItemsToDataArray(Series series)
        {
            var resultSeries = new Series()
            {
                DataArray = new DataArray(series.Size)
            };

            for (var i = 0; i < series.Size; i++)
            {
                var data = series.Items[i];

                for (var j = 0; j < 12; j++)
                {
                    resultSeries.DataArray[i][j] = data[j];
                }
            }

            return resultSeries.DataArray;
        }
    }
}
