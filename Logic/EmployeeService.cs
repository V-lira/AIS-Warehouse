using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.Logic
{
    public class EmployeeService
    {
        private readonly EmployeeRepository2 _repository;

        public EmployeeService()
        {
            _repository = new EmployeeRepository2();
        }

        public List<UserDto> GetAll() => _repository.GetAll();
        public List<UserDto> Search(string text) => _repository.Search(text);
        public void Add(UserDto user) => _repository.Add(user);
        public void Delete(int id) => _repository.Delete(id);
    }
}