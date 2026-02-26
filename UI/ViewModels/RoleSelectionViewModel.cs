using AIS.Warehouse.UI.Commands;
using AIS.Warehouse.UI.Models;
using AIS.Warehouse.UI.Services;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class RoleSelectionViewModel
    {
        private readonly INavigationService _navigation;

        public ICommand AdminCommand { get; }
        public ICommand EmployeeCommand { get; }
        public ICommand SupplierCommand { get; }

        public RoleSelectionViewModel(INavigationService navigation)
        {
            _navigation = navigation;

            AdminCommand = new RelayCommand(_ => _navigation.OpenLogin(UserRole.Admin));
            EmployeeCommand = new RelayCommand(_ => _navigation.OpenLogin(UserRole.Employee));
            SupplierCommand = new RelayCommand(_ => _navigation.OpenLogin(UserRole.Supplier));
        }
    }
}
