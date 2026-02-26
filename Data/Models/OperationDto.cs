using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.Data.Models
{
    public class OperationDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public string OperationType { get; set; }
        public DateTime OperationDate { get; set; }
    }
}