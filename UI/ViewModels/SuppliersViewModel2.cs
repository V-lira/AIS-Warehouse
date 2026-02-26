using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class SuppliersViewModel2 : BaseViewModel
    {
        private readonly SupplierRepository _repo = new SupplierRepository();

        public ObservableCollection<SupplierDto> Suppliers { get; } = new ObservableCollection<SupplierDto>();

        private SupplierDto _selectedSupplier;
        public SupplierDto SelectedSupplier
        {
            get => _selectedSupplier;
            set { _selectedSupplier = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); Load(); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public SuppliersViewModel2()
        {
            AddCommand = new RelayCommand(_ => Add());
            EditCommand = new RelayCommand(_ => Edit(), _ => SelectedSupplier != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedSupplier != null);

            Load();
        }

        private void Load()
        {
            Suppliers.Clear();

            var data = string.IsNullOrWhiteSpace(SearchText)
                ? _repo.GetAll()
                : _repo.Search(SearchText);

            foreach (var s in data)
                Suppliers.Add(s);
        }

        private void Add()
        {
            var s = new SupplierDto
            {
                FirstName = "Новый",
                LastName = "Поставщик",
                DepartmentId = 1,
                Login = "new@au.com"
            };

            _repo.Add(s);
            Load();
        }

        private void Edit()
        {
            _repo.Update(SelectedSupplier);
            Load();
        }

        private void Delete()
        {
            _repo.Delete(SelectedSupplier.Id);
            Suppliers.Remove(SelectedSupplier);
        }
    }
}
