using System;
using CBA.BusinessLogic.Models;

namespace CBA.BusinessLogic.Logic
{
    public class DataInitialiser
    {
        public void FillNumbersByDefault(Series[] series, int size, int sizeOfRows)
        {
            var random = new Random(unchecked((int) DateTime.Now.Ticks));

            for (var k = 0; k < size; k++)
            {
                for (var i = 0; i < sizeOfRows; i++)
                {
                    for (var j = 0; j < 12; j++)
                    {
                        series[k].DataArray[i][j] = random.Next(7, 12);

                        if ((j + 1)%3 == 0)
                        {
                            series[k].DataArray[i][j] = 0; //i row j column
                        }
                    }
                }
            }
        }

        public void Load(Series[] series, DataArray[] a)
        {
            for (var k = 0; k < 3; k++)
            {
                for (var i = 0; i < series[0].Size; i++)
                {
                    for (var j = 0; j < 12; j++)
                    {
                        a[k][i][j] = series[k].DataArray[i][j];
                    }
                }
            }
        }
    }
}