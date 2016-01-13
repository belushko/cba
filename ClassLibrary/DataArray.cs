using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace cba
{
    public class DataArray
    {
        DataTable data = new DataTable(); 
        int m, n;                        

        //кол-во строк
        public int M { get { return m; } }

        //кол-во столбцов
        public int N { get { return n; } }

        public DataTable Data
        {
            get { return data; }
        }

        public class DataArrayRow
        {
            DataTable data;
            public int row;

            public DataArrayRow(DataTable data, int row)
            {
                this.row = row;
                this.data = data;
            }

            public double this[int index]
            {
                get { return double.Parse(data.Rows[row][index] + ""); }
                set { data.Rows[row].SetField<double>(index, value); }
            }
        }

        public DataArrayRow this[int index]
        {
            get { return new DataArrayRow(data, index); }
        }

        public DataArray()
        { }

        public DataArray(int m, int n)
        {
            this.m = m;
            this.n = n;
            
            //добавление колонок с названием (номер):
            for (int j = 0; j < 12; j++)
            {
                string w = "";
                if ((j + 1) % 3 == 0)
                {
                    w = "Среднее ";
                }

                data.Columns.Add(w + (j + 1) + "");
                w = "";
            }

            for (int i = 0; i < n; i++)
            { 
                data.Rows.Add();
            }

            for (int i = 0; i < m; i++)
            {
                if ((i + 1) % 3 == 0)
                {
                    //data.Columns[i].ReadOnly = true;                     
                }
            }

        }
    }
}
