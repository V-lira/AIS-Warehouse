using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using System.Collections.ObjectModel;

namespace AIS.Warehouse.UI.ViewModels
{
    public class CompaniesViewModel
    {
        public ObservableCollection<CompanyDto> Companies { get; }

        public CompaniesViewModel()
        {
            var repo = new CompanyRepository();
            Companies = new ObservableCollection<CompanyDto>(repo.GetAll());
        }
    }
}