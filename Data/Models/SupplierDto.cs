using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.Data.Models
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public int DepartmentId { get; set; }
        public string Login { get; set; }
        public DateTime DateOfRegistration { get; set; }

        // Вычисляемое свойство для отображения
        public string FullName => $"{LastName} {FirstName} {Initials}";
    }
}