using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace AIS.Warehouse.Logic
{
    public class ExcelService
    {
        public DataTable Read(string path)
        {
            var table = new DataTable();

            using (var workbook = new XLWorkbook(path))
            {
                var ws = workbook.Worksheet(1);
                var firstRow = ws.FirstRowUsed();

                foreach (var cell in firstRow.Cells())
                    table.Columns.Add(cell.Value.ToString());

                foreach (var row in ws.RowsUsed().Skip(1))
                {
                    var dataRow = table.NewRow();
                    for (int i = 0; i < table.Columns.Count; i++)
                        dataRow[i] = row.Cell(i + 1).Value.ToString();
                    table.Rows.Add(dataRow);
                }
            }

            return table;
        }
    }
}