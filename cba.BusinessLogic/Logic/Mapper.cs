using System.Collections.Generic;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class Mapper
    {
        public List<double[]> DataArrayToItems(DataArray dataArray)
        {
            var size = dataArray?.Data?.Rows?.Count;
            var columnsNumber = dataArray.Data.Columns.Count;

            if (size == null || columnsNumber == 0)
            {
                return new List<double[]>();
            }

            var resultSeries = new Series
            {
                Items = new List<double[]>()
            };

            for (var i = 0; i < size; i++)
            {
                var data = new double[columnsNumber];

                for (var j = 0; j < columnsNumber; j++)
                {
                    data[j] = dataArray[i][j];
                }

                resultSeries.Items.Add(data);
            }

            return resultSeries.Items;
        }

        public DataArray ItemsToDataArray(List<double[]> items)
        {
            var size = items.Count;

            if (size == 0)
            {
                return new DataArray();
            }

            var resultSeries = new Series()
            {
                DataArray = new DataArray(size)
            };

            for (var i = 0; i < size; i++)
            {
                var data = items[i];

                for (var j = 0; j < 12; j++)
                {
                    resultSeries.DataArray[i][j] = data[j];
                }
            }

            return resultSeries.DataArray;
        }
    }
}
