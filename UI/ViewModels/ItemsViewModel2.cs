using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class ItemsViewModel2 : BaseViewModel
    {
        private readonly ItemRepository _repository = new ItemRepository();

        public ObservableCollection<ItemDto> Items { get; } = new ObservableCollection<ItemDto>();

        private ItemDto _selectedItem;
        public ItemDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
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
                LoadItems();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ResetSearchCommand { get; }

        public ItemsViewModel2()
        {
            AddCommand = new RelayCommand(_ => AddItem());
            EditCommand = new RelayCommand(_ => EditItem(), _ => SelectedItem != null);
            DeleteCommand = new RelayCommand(_ => DeleteItem(), _ => SelectedItem != null);
            ResetSearchCommand = new RelayCommand(_ => SearchText = string.Empty);

            LoadItems();
        }

        private void LoadItems()
        {
            Items.Clear();

            var list = string.IsNullOrWhiteSpace(SearchText)
                ? _repository.GetAll()
                : _repository.Search(SearchText);

            foreach (var i in list)
                Items.Add(i);
        }

        private void AddItem()
        {
            var item = new ItemDto
            {
                ItemCode = "NEW-001",
                Name = "Новый товар",
                Price = 0,
                CompanyId = 1
            };

            _repository.Add(item);
            LoadItems();
        }

        private void EditItem()
        {
            if (SelectedItem == null) return;

            _repository.Update(SelectedItem);
            LoadItems();
        }

        private void DeleteItem()
        {
            if (SelectedItem == null) return;

            _repository.Delete(SelectedItem.Id);
            Items.Remove(SelectedItem);
        }
    }
}
