using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace cba
{
    public class Series
    {
        public string Name { set; get; }
        public int SizeOfSeries { set; get; }//number of series (tables)
        public int Size { set; get; }//number of animals (rows)
        public List<double[]> Mas = new List<double[]>(); //table values
        public DataArray a;
        public double[] breeding = new double[4];//breeding sum values
        public double MOValue { set; get; }
        public double ratio;//efficiency ratio
        public double activity;//biological activity

        public Series()
        { }

        public Series(int size)
        {
            Size = size;
            a = new DataArray(size, size);
            Mas = new List<double[]>();
        }

        public void InitTable()
        {
            Random random = new Random(unchecked((int)(DateTime.Now.Ticks)));

            for (int i = 0; i < 2; i++)
            {

                for (int j = 0; j < 12; j++)
                {
                    if ((j + 1) % 3 != 0)
                    {
                        a[i][j] = int.Parse(random.Next(7, 12) + "");
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

            for (int i = 0; i < a.M; i++)
            {
                breeding[0] += a[i][2];
                breeding[1] += a[i][5];
                breeding[2] += a[i][8];
                breeding[3] += a[i][11];
            }
        }

        public void FindMidValues()
        {
            for (int i = 0; i < a.M; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if ((j + 1) % 3 == 0)
                    {
                        a[i][j] = (a[i][j - 2] + a[i][j - 1]) / 2;
                    }
                }
            }
        }

        public void FindActivityValue()
        {
            activity = (breeding[0] + breeding[1]) / (breeding[2] + breeding[3]) * MOValue;
        }
    }
}
