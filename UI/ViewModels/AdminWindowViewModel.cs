using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using AIS.Warehouse.UI.Services;
using AIS.Warehouse.UI.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class AdminWindowViewModel2 : BaseViewModel
    {
        private readonly UserSession _session;
        private readonly INavigationService _navigationService;

        // Репозитории
        private readonly EmployeeRepository2 _employeeRepository;
        private readonly SupplierRepository _supplierRepository;
        private readonly ItemRepository _itemRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly OperationRepository _operationRepository;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public string WelcomeText => $"Добро пожаловать, {_session.FirstName}!";
        public string CurrentDate => DateTime.Now.ToString("dd.MM.yyyy");

        public ICommand OpenEmployeesCommand { get; }
        public ICommand OpenSuppliersCommand { get; }
        public ICommand OpenItemsCommand { get; }
        public ICommand OpenCompaniesCommand { get; }
        public ICommand OpenOperationsCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminWindowViewModel2(UserSession session, INavigationService navigationService)
        {
            _session = session;
            _navigationService = navigationService;

            // Инициализация репозиториев
            _employeeRepository = new EmployeeRepository2();
            _supplierRepository = new SupplierRepository();
            _itemRepository = new ItemRepository();
            _companyRepository = new CompanyRepository();
            _operationRepository = new OperationRepository("ваша_строка_подключения");

            // Инициализация команд
            OpenEmployeesCommand = new RelayCommand(_ => OpenEmployees());
            OpenSuppliersCommand = new RelayCommand(_ => OpenSuppliers());
            OpenItemsCommand = new RelayCommand(_ => OpenItems());
            OpenCompaniesCommand = new RelayCommand(_ => OpenCompanies());
            OpenOperationsCommand = new RelayCommand(_ => OpenOperations());
            LogoutCommand = new RelayCommand(_ => Logout());

            // Открываем первую вкладку по умолчанию
            OpenEmployees();
        }

        private void OpenEmployees()
        {
            CurrentView = new EmployeesView
            {
                DataContext = new EmployeesViewModel(_employeeRepository)
            };
        }

        private void OpenSuppliers()
        {
            CurrentView = new SuppliersView2
            {
                DataContext = new SuppliersViewModel2()
            };
        }

        private void OpenItems()
        {
            CurrentView = new ItemsView2
            {
                DataContext = new ItemsViewModel2()
            };
        }

        private void OpenCompanies()
        {
            CurrentView = new CompaniesView2
            {
                DataContext = new CompaniesViewModel2()
            };
        }

        private void OpenOperations()
        {
            CurrentView = new OperationsView2
            {
                DataContext = new OperationsViewModel2(_operationRepository)
            };
        }

        private void Logout()
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _navigationService.Logout();
            }
        }
    }
}
