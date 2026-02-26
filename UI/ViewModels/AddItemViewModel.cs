using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.UI.Commands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private readonly ItemRepository _itemRepo;
        private string _itemCode;
        public string ItemCode
        {
            get => _itemCode;
            set
            {
                _itemCode = value;
                OnPropertyChanged(nameof(ItemCode));
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        private int _companyId;
        public int CompanyId
        {
            get => _companyId;
            set
            {
                _companyId = value;
                OnPropertyChanged(nameof(CompanyId));
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public AddItemViewModel()
        {
            string connectionString = @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";
            _itemRepo = new ItemRepository(connectionString);
            ItemCode = string.Empty;
            Name = string.Empty;
            Price = 0;
            CompanyId = 0;
            SaveCommand = new RelayCommand(_ => SaveItem());
            CancelCommand = new RelayCommand(_ => Cancel());
        }
        private void SaveItem()
        {
            try
            {
                MessageBox.Show($"Отладка:\nItemCode: '{ItemCode}'\nName: '{Name}'\nPrice: {Price}\nCompanyId: {CompanyId}");
                //ВАЛИДАЦИЯ:
                if (string.IsNullOrWhiteSpace(ItemCode))
                {
                    MessageBox.Show("Введите артикул товара");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Name))
                {
                    MessageBox.Show("Введите название товара");
                    return;
                }
                if (Price < 0)
                {
                    MessageBox.Show("Цена не может быть отрицательной!");
                    return;
                }
                var newItem = new ItemDto
                {
                    ItemCode = ItemCode.Trim(),
                    Name = Name.Trim(),
                    Price = Price,
                    CompanyId = CompanyId
                };
                //сохранение в бд:
                _itemRepo.Create(newItem);
                MessageBox.Show($"Товар '{Name}' успешно добавлен!\nАртикул: {ItemCode}","Успех",MessageBoxButton.OK,MessageBoxImage.Information);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении товара: {ex.Message}\n\n{ex.StackTrace}","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            ClearFields();
            MessageBox.Show("Создание товара отменено");
        }
        private void ClearFields()
        {
            ItemCode = string.Empty;
            Name = string.Empty;
            Price = 0;
            CompanyId = 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}