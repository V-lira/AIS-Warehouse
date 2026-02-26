using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Data.Repositories;
using System.Collections.ObjectModel;

namespace AIS.Warehouse.UI.ViewModels
{
    public class ContactsViewModel
    {
        public ObservableCollection<AdminContactDto> Admins { get; }

        public ContactsViewModel()
        {
            var repo = new AdminRepository();
            Admins = new ObservableCollection<AdminContactDto>(repo.GetAllAdmins());
        }
    }
}