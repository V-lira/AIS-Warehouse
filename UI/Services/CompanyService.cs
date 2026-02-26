using AIS.Warehouse.Data.Repositories;
using System;
using System.Collections.Generic;
using AIS.Warehouse.Data.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.UI.Services
{
    public class CompanyService
    {
        private readonly CompanyRepository _repo;

        public CompanyService()
        {
            //string connectionString = @"Server=DESKTOP-HPIB8IV\SQLEXPRESS;Database=AIS_Warehouse;Trusted_Connection=True;TrustServerCertificate=True;";
            _repo = new CompanyRepository(/*connectionString*/);
        }

        public CompanyDto GetMainCompany()
        {
            // Возвращаем первую компанию из списка
            var companies = _repo.GetAll();
            return companies.FirstOrDefault() ?? new CompanyDto
            {
                Id = 1,
                Name = "ООО «Золотой Ящик»",
                Address = "г. Москва, ул. Складская, 15",
                INN = "7701234567"
            };
        }

        public List<CompanyDto> GetAllCompanies()
        {
            return _repo.GetAll();
        }
    }
}