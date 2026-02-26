using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class AddOperationViewModel2 : INotifyPropertyChanged
    {
        private readonly OperationRepository _operationRepo;
        private readonly ItemRepository _itemRepo;

        public ObservableCollection<ItemDto> Items { get; }
        public ObservableCollection<string> OperationTypes { get; }

        private ItemDto _selectedItem;
        public ItemDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private string _selectedOperationType;
        public string SelectedOperationType
        {
            get => _selectedOperationType;
            set
            {
                _selectedOperationType = value;
                OnPropertyChanged(nameof(SelectedOperationType));
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Window Window { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddOperationViewModel2(
            OperationRepository operationRepo,
            ItemRepository itemRepo)
        {
            _operationRepo = operationRepo;
            _itemRepo = itemRepo;

            Items = new ObservableCollection<ItemDto>();
            var items = _itemRepo.GetAll();
            foreach (var item in items)
                Items.Add(item);

            OperationTypes = new ObservableCollection<string>
            {
                "IN",
                "OUT"
            };

            SaveCommand = new RelayCommand(_ => Save(), CanSave);
            CancelCommand = new RelayCommand(_ => Window?.Close());
        }

        private bool CanSave(object parameter)
        {
            return SelectedItem != null &&
                   !string.IsNullOrEmpty(SelectedOperationType) &&
                   Quantity > 0;
        }

        private void Save()
        {
            if (!CanSave(null))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Здесь должен быть ваш реальный UserId
                int currentUserId = 1; // Замените на актуальный ID пользователя

                _operationRepo.AddOperation(
                    SelectedItem.Id,
                    currentUserId,
                    SelectedOperationType,
                    Quantity
                );

                if (Window != null)
                {
                    Window.DialogResult = true;
                    Window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении операции:\n{ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
    }
}