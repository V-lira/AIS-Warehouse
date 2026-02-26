using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using System.Collections.ObjectModel;

namespace AIS.Warehouse.UI.ViewModels
{
    public class SuppliersViewModel
    {
        public ObservableCollection<SupplierDto> Suppliers { get; }

        public SuppliersViewModel()
        {
            var repo = new SupplierRepository();
            Suppliers = new ObservableCollection<SupplierDto>(repo.GetAll());
        }
    }
}