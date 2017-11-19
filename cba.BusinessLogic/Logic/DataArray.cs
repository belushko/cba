using System.Data;

namespace cba.Logic
{
    public class DataArray
    {
        public DataArray()
        {
        }

        public DataArray(int m)
        {
            var number = 0;
            //добавление колонок с названием (номер):
            for (var j = 0; j < 12; j++)
            {
                number++;
                if ((j + 1)%3 == 0)
                {
                    Data.Columns.Add(new DataColumn {ColumnName = (j + 1).ToString()});
                    number = 0;
                }
                else
                {
                    Data.Columns.Add(new DataColumn {ColumnName = (j + 1).ToString()});
                }
            }

            for (var i = 0; i < m; i++)
            {
                Data.Rows.Add();
            }
        }

        public DataTable Data { get; } = new DataTable();

        public DataArrayRow this[int index]
        {
            get { return new DataArrayRow(Data, index); }
        }

        public class DataArrayRow
        {
            private readonly DataTable data;
            public int row;

            public DataArrayRow(DataTable data, int row)
            {
                this.row = row;
                this.data = data;
            }

            public double this[int index]
            {
                get { return double.Parse(data.Rows[row][index] + ""); }
                set { data.Rows[row].SetField(index, value); }
            }
        }
    }
}