using AIS.Warehouse.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.Data.Models
{
    public class UserSession
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public UserRole Role { get; set; }
    }
}
