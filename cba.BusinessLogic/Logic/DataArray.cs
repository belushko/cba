using System.Data;

namespace CBA.BusinessLogic.Logic
{
    public class DataArray
    {
        public DataTable Data { get; } = new DataTable();

        public DataArrayRow this[int index]
        {
            get { return new DataArrayRow(Data, index); }
        }

        public DataArray()
        {
        }

        public DataArray(int rowsCount)
        {
            var columnsCount = 12;

            var number = 0;

            //добавление колонок с названием (номер):
            for (var j = 0; j < columnsCount; j++)
            {
                number++;

                if ((j + 1) % 3 == 0)
                {
                    Data.Columns.Add(new DataColumn {ColumnName = (j + 1).ToString()});
                    number = 0;
                }
                else
                {
                    Data.Columns.Add(new DataColumn {ColumnName = (j + 1).ToString()});
                }
            }

            for (var i = 0; i < rowsCount; i++)
            {
                Data.Rows.Add();
            }
        }      
    }
}