using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using System.Collections.Generic;

namespace AIS.Warehouse.Logic
{
    public class ItemService
    {
        private readonly ItemRepository _repository;

        public ItemService()
        {
            _repository = new ItemRepository(); // ИСПРАВЛЕНО: без new()
        }

        public List<ItemDto> GetAll() => _repository.GetAll();
        public List<ItemDto> Search(string text) => _repository.Search(text);
        public void Add(ItemDto item) => _repository.Add(item); // У вас метод Add, не Create!
        public void Update(ItemDto item) => _repository.Update(item);
        public void Delete(int id) => _repository.Delete(id);
    }
}