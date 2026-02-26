using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AIS.Warehouse.Logic;

namespace AIS.Warehouse.UI.ViewModels
{
    public class ExcelViewModel
    {
        public DataTable Rows { get; }

        public ExcelViewModel(string path)
        {
            var service = new ExcelService();
            Rows = service.Read(path);
        }
    }
}