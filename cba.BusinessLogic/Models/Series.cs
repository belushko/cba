using System.Collections.Generic;
using CBA.BusinessLogic.Logic;

namespace CBA.BusinessLogic.Models
{
    public class Series
    {
        public DataArray DataArray { get; set; }

        public List<double[]> Items; //table values

        public double Activity { get; set; } //biological Activity

        public double[] Breeding { get; set; } = new double[4]; //Breeding sum values

        public double Ratio { get; set; } //efficiency Ratio

        public int SizeOfSeries { set; get; } //number of series (tables)

        public int Size { set; get; } //number of animals (rows)

        public double MOValue { set; get; }

        public string Name { set; get; }

        public Series()
        { }

        public Series(int size)
        {
            DataArray = new DataArray(size);
        }      
    }
}