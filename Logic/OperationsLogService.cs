using AIS.Warehouse.Data.Models;
using System.Collections.Generic;
using AIS.Warehouse.Data.Repositories;

namespace AIS.Warehouse.Logic
{
    public class OperationsLogService
    {
        private readonly OperationsLogRepository _repo;

        public OperationsLogService(string connectionString)
        {
            _repo = new OperationsLogRepository(connectionString);
        }

        public List<OperationDto> GetAll()
        {
            return _repo.GetAll();
        }

        public List<OperationDto> Search(string text)
        {
            return _repo.Search(text);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}