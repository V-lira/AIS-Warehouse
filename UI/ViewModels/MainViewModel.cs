using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Logic;
using AIS.Warehouse.UI.Commands;
using AIS.Warehouse.UI.Models;
using AIS.Warehouse.UI.Services;
using AIS.Warehouse.UI.Views;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigation;
        public string WelcomeText { get; }
        public string CurrentDate => DateTime.Now.ToShortDateString();
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        private readonly UserSession _session;
        private readonly ReportService _reportService;
        private readonly WordReportService _wordReportService;
        public ICommand OpenAddItemCommand { get; }
        public ICommand OpenOperationsCommand { get; }
        public ICommand OpenAddOperationCommand { get; }
        public ICommand CreateExcelReportCommand { get; }
        public ICommand CreateWordReportCommand { get; }
        public ICommand OpenExcelFileCommand { get; }
        public ICommand OpenCompanyInfoCommand { get; }
        public ICommand OpenItemsCommand { get; }
        public ICommand OpenCompaniesCommand { get; }
        public ICommand OpenSuppliersCommand { get; }
        public ICommand OpenContactsCommand { get; }
        public ICommand OpenEmployeesCommand { get; }

        public MainViewModel(UserSession session, INavigationService navigation)
        {
            _session = session;
            WelcomeText = $"Добро пожаловать, {session.FirstName}! ({session.Role})";
            //командики
            OpenOperationsCommand = new RelayCommand(_ => OpenOperations());
            OpenAddOperationCommand = new RelayCommand(_ => OpenAddOperation());
            OpenCompanyInfoCommand = new RelayCommand(_ => OpenAboutCompany());
            OpenItemsCommand = new RelayCommand(_ => OpenItems());
            OpenEmployeesCommand = new RelayCommand(_ => OpenEmployeesCount());
            OpenExcelFileCommand = new RelayCommand(_ => OpenExcelFile());
            OpenAddItemCommand = new RelayCommand(_ => OpenAddItem());
            //сервисы мои
            _reportService = new ReportService();
            _wordReportService = new WordReportService();
            //отчётики мои
            CreateExcelReportCommand = new RelayCommand(_ => CreateExcelReport());
            CreateWordReportCommand = new RelayCommand(_ => CreateWordReport());
            //таблички мои
            OpenCompaniesCommand = new RelayCommand(_ => OpenCompanies());
            OpenSuppliersCommand = new RelayCommand(_ => OpenSuppliers());
            OpenContactsCommand = new RelayCommand(_ => OpenContacts());
            OpenOperations();
        }
        private void OpenAddItem()
        {
            CurrentView = new AddItemView
            {
                DataContext = new AddItemViewModel()
            };
        }
        private void OpenContacts()
        {
            CurrentView = new ContactsView
            {
                DataContext = new ContactsViewModel()
            };
        }
        private void OpenSuppliers()
        {
            CurrentView = new SuppliersView
            {
                DataContext = new SuppliersViewModel()
            };
        }
        private void OpenCompanies()
        {
            CurrentView = new CompaniesView
            {
                DataContext = new CompaniesViewModel()
            };
        }
        public void OpenOperations()
        {
            CurrentView = new OperationsView
            {
                DataContext = new OperationsViewModel()
            };
        }
        private void CreateExcelReport()
        {
            try
            {
                _reportService.CreateOperationsExcel(_session.UserId);
                MessageBox.Show("Excel-файл успешно создан на рабочем столе");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании Excel:\n" + ex.Message);
            }
        }
        public void OpenAddOperation()
        {
            CurrentView = new AddOperationView
            {
                DataContext = new AddOperationViewModel(_session)
            };
        }
        private void CreateWordReport()
        {
            try
            {
                _wordReportService.CreateOperationsWord(_session.UserId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании Word-отчета: {ex.Message}");
            }
        }
        private void OpenAboutCompany()
        {
            CurrentView = new AboutCompanyView
            {
                DataContext = new AboutCompanyViewModel()
            };
        }
        private void OpenItems()
        {
            CurrentView = new ItemsView
            {
                DataContext = new ItemsViewModel()
            };
        }
        private void OpenEmployeesCount()
        {
            //данные для роли в ENUM!!!
            if (_session.Role != UserRole.Admin)
            {
                MessageBox.Show($"Доступ запрещён!\n" + $"Только администраторы могут просматривать сотрудников.\n" + $"Ваша роль: {_session.Role}","Ошибка доступа", MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            //CurrentView = new EmployeesCountView
            //{
            //    DataContext = new EmployeesCountViewModel()
            //};
        }
        private void OpenExcelFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Excel файлы (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                CurrentView = new ExcelView
                {
                    DataContext = new ExcelViewModel(dialog.FileName)
                };
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}