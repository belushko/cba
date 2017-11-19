using System.Data;

namespace CBA.BusinessLogic.Logic
{
    public class DataArrayRow
    {
        private readonly DataTable data;
        public int row;

        public double this[int index]
        {
            get { return double.Parse(data.Rows[row][index] + ""); }
            set { data.Rows[row].SetField(index, value); }
        }

        public DataArrayRow(DataTable data, int row)
        {
            this.row = row;
            this.data = data;
        }      
    }
}
