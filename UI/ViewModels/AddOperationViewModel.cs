using AIS.Warehouse.Data;
using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using AIS.Warehouse.Logic;
using AIS.Warehouse.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static AIS.Warehouse.Data.Repositories.ItemRepository;

namespace AIS.Warehouse.UI.ViewModels
{
    public class AddOperationViewModel
    {
        private readonly OperationService _service;
        private readonly ItemRepository _itemRepo;
        private readonly UserSession _session;
        public ObservableCollection<ItemDto> Items { get; }
        public ItemDto SelectedItem { get; set; }
        public ObservableCollection<string> OperationTypes { get; }
        public string SelectedOperationType { get; set; }
        public int Quantity { get; set; }
        public ICommand SaveCommand { get; }
        public AddOperationViewModel(UserSession session)
        {
            _session = session;
            _service = new OperationService();
            //подключаю бдшку для доступа
            string connectionString = Data.DbConnectionFactory.ConnectionString;
            _itemRepo = new ItemRepository(connectionString);
            //загружаю данные благодаря другому модолю с товарами + коннект бдшки
            var itemsList = _itemRepo.GetAll();
            Items = new ObservableCollection<ItemDto>(itemsList);
            //задаю что именно IN и OUT
            OperationTypes = new ObservableCollection<string> { "IN", "OUT" };
            SaveCommand = new RelayCommand(_ => Save());

            //чек
            //if (Items.Count > 0)
            //{
            //    MessageBox.Show($"Загружено товаров: {Items.Count}\n" + $"Первый товар: ID={Items.First().Id}, Name={Items.First().Name}");
            //}
            //else
            //{
            //    MessageBox.Show("Внимание: не загружены товары");
            //}
        }

        private void Save()
        {
            try
            {
                if (SelectedItem == null)
                {
                    MessageBox.Show("Выберите товар");
                    return;
                }

                if (string.IsNullOrEmpty(SelectedOperationType))
                {
                    MessageBox.Show("Выберите тип операции");
                    return;
                }

                if (Quantity <= 0)
                {
                    MessageBox.Show("Количество должно быть больше 0");
                    return;
                }
                //по чеку чекает
                string dbOperationType = SelectedOperationType.ToUpper();
                //доп проверка:
                if (dbOperationType != "IN" && dbOperationType != "OUT")
                {
                    MessageBox.Show($"Тип операции должен быть 'IN' или 'OUT', получено: '{SelectedOperationType}'");
                    return;
                }
                //чек
                MessageBox.Show($"Сохранение:\n" + $"Товар: {SelectedItem.Name} (ID={SelectedItem.Id})\n Тип: {dbOperationType}\n Кол-во: {Quantity}\n UserId: {_session.UserId}");
                //тут некий сервис для создания CREATE
                _service.CreateOperation(
                    //реальный айдишник
                    SelectedItem.Id,
                    //айди юзера
                    _session.UserId,
                    //IN or OUT
                    dbOperationType,
                    //кол-во
                    Quantity
                );
                MessageBox.Show("Операция добавлена!");
                SelectedItem = null;
                SelectedOperationType = null;
                Quantity = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n {ex.InnerException?.Message}");
            }
        }
    }
}