using System;

namespace cba.Logic
{
    public class Series
    {
        public DataArray a;
        public double activity; //biological activity
        public double[] breeding = new double[4]; //breeding sum values
        public double ratio; //efficiency ratio

        public Series()
        {
        }

        public Series(int size)
        {
            Size = size;
            a = new DataArray(size);
        }

        public static int SizeOfSeries { set; get; } //number of series (tables)
        public static int Size { set; get; } //number of animals (rows)
        public static double MOValue { set; get; }


        public string Name { set; get; }


        public void InitTable()
        {
            var random = new Random(unchecked((int) DateTime.Now.Ticks));

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 12; j++)
                {
                    if ((j + 1)%3 != 0)
                    {
                        a[i][j] = int.Parse(random.Next(100, 120) + "");
                    }
                }
            }
        }

        public void FindBreedingSum()
        {
            breeding[0] = 0;
            breeding[1] = 0;
            breeding[2] = 0;
            breeding[3] = 0;

            for (var i = 0; i < Size; i++)
            {
                breeding[0] += a[i][2];
                breeding[1] += a[i][5];
                breeding[2] += a[i][8];
                breeding[3] += a[i][11];
            }
        }

        public void FindMidValues()
        {
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < 12; j++)
                {
                    if ((j + 1)%3 == 0)
                    {
                        a[i][j] = (a[i][j - 2] + a[i][j - 1])/2;
                    }
                }
            }
        }

        public void FindActivityValue()
        {
            activity = (breeding[0] + breeding[1])/(breeding[2] + breeding[3])*MOValue;
        }
    }
}