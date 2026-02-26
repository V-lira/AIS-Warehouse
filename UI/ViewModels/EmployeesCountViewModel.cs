using AIS.Warehouse.Data.Models;
using AIS.Warehouse.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.UI.ViewModels
{
    public class EmployeesCountViewModel
    {
        public ObservableCollection<UserDto> Employees { get; }
        public int TotalCount => Employees.Count;

        public string Summary => $"Всего сотрудников: {TotalCount}";

        public EmployeesCountViewModel()
        {
            var service = new UserService();
            var employees = service.GetAllEmployees();
            Employees = new ObservableCollection<UserDto>(employees);
        }
    }
}