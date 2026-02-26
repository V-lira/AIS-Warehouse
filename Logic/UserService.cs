using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Repositoties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.Logic
{
    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService()
        {
            string connectionString = @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";
            _repo = new UserRepository(connectionString);
        }

        public List<UserDto> GetAllEmployees()
        {
            return _repo.GetAllEmployees();
        }
    }
}