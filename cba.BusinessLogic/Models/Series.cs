using CBA.BusinessLogic.Logic;

namespace CBA.BusinessLogic.Models
{
    public class Series
    {
        public DataArray a;
        public double activity; //biological activity
        public double[] breeding = new double[4]; //breeding sum values
        public double ratio; //efficiency ratio
        
        public int SizeOfSeries { set; get; } //number of series (tables)
        public int Size { set; get; } //number of animals (rows)
        public double MOValue { set; get; }
        public string Name { set; get; }

        public Series()
        {
        }

        public Series(int size)
        {
            a = new DataArray(size);
        }      
    }
}