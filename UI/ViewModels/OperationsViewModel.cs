using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.UI.ViewModels
{
    public class OperationsViewModel
    {
        public ObservableCollection<OperationDto> Operations { get; }

        public OperationsViewModel()
        {
            var service = new OperationService();
            Operations = new ObservableCollection<OperationDto>(
                service.GetJournal()
            );
        }
    }
}
