using AIS.Warehouse.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AIS.Warehouse.Logic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.UI.ViewModels
{
    public class ItemsViewModel
    {
        public ObservableCollection<ItemDto> Items { get; }

        public ItemsViewModel()
        {
            var service = new ItemService();
            Items = new ObservableCollection<ItemDto>(service.GetAll());
        }
    }
}