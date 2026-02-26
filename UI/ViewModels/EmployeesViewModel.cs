using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class EmployeesViewModel : BaseViewModel
    {
        private readonly EmployeeRepository2 _repository;

        public ObservableCollection<UserDto> Employees { get; } = new ObservableCollection<UserDto>();

        private UserDto _selectedEmployee;
        public UserDto SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                LoadEmployees();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ResetSearchCommand { get; }

        public EmployeesViewModel(EmployeeRepository2 repository)
        {
            _repository = repository;

            AddCommand = new RelayCommand(_ => AddEmployee());
            DeleteCommand = new RelayCommand(_ => DeleteEmployee(), _ => SelectedEmployee != null);
            ResetSearchCommand = new RelayCommand(_ => ResetSearch());

            LoadEmployees();
        }

        private void LoadEmployees()
        {
            Employees.Clear();
            var list = string.IsNullOrEmpty(SearchText) ? _repository.GetAll() : _repository.Search(SearchText);
            foreach (var emp in list)
                Employees.Add(emp);
        }

        private void AddEmployee()
        {
            var user = new UserDto
            {
                FirstName = "Новое",
                LastName = "Имя",
                Initials = "",
                Login = "login@au.com",
                PasswordHash = "123"
            };
            _repository.Add(user);
            LoadEmployees();
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee == null) return;

            _repository.Delete(SelectedEmployee.Id);
            Employees.Remove(SelectedEmployee);
        }

        private void ResetSearch()
        {
            SearchText = string.Empty;
        }
    }
}
