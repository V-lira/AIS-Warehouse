using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class CompaniesViewModel2 : BaseViewModel
    {
        private readonly CompanyRepository _repository = new CompanyRepository();

        public ObservableCollection<CompanyDto> Companies { get; } = new ObservableCollection<CompanyDto>();

        private CompanyDto _selectedCompany;
        public CompanyDto SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                _selectedCompany = value;
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
                LoadCompanies();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ResetSearchCommand { get; }

        public CompaniesViewModel2()
        {
            AddCommand = new RelayCommand(_ => AddCompany());
            EditCommand = new RelayCommand(_ => EditCompany(), _ => SelectedCompany != null);
            DeleteCommand = new RelayCommand(_ => DeleteCompany(), _ => SelectedCompany != null);
            ResetSearchCommand = new RelayCommand(_ => SearchText = string.Empty);

            LoadCompanies();
        }

        private void LoadCompanies()
        {
            Companies.Clear();

            var list = string.IsNullOrWhiteSpace(SearchText)
                ? _repository.GetAll()
                : _repository.Search(SearchText);

            foreach (var c in list)
                Companies.Add(c);
        }

        private void AddCompany()
        {
            var company = new CompanyDto
            {
                INN = "0000000000",
                Name = "Новая компания",
                Address = "Адрес"
            };

            _repository.Add(company);
            LoadCompanies();
        }

        private void EditCompany()
        {
            if (SelectedCompany == null) return;

            _repository.Update(SelectedCompany);
            LoadCompanies();
        }

        private void DeleteCompany()
        {
            if (SelectedCompany == null) return;

            _repository.Delete(SelectedCompany.Id);
            Companies.Remove(SelectedCompany);
        }
    }
}
