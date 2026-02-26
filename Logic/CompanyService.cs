using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using System.Collections.Generic;

namespace AIS.Warehouse.Logic
{
    public class CompanyService
    {
        private readonly CompanyRepository _repo;

        public CompanyService()
        {
            _repo = new CompanyRepository();
        }

        public List<CompanyDto> GetAllCompanies()
        {
            return _repo.GetAll();
        }
    }
}
