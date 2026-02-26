using AIS.Warehouse.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.Logic
{
    public class LogService
    {
        private readonly LogRepository _repo;

        public LogService()
        {
            // Нужно передать connectionString в конструктор!
            string connectionString = @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";
            _repo = new LogRepository(connectionString);
        }

        public void Log(int userId, string action, string entity, int? entityId = null)
        {
            _repo.Add(userId, action, entity, entityId);
        }
    }
}