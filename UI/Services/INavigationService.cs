using AIS.Warehouse.Data.Models;
using AIS.Warehouse.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIS.Warehouse.UI.Services
{
    //для чего это?
    //это интерфейс навигации между окнами. но не реализует их.
    //"между экранами -> такое-то такое-то" - вот что он говррит
    public interface INavigationService
    {
        void OpenLogin(UserRole role);
        void OpenMainWindow(UserSession session);
        void OpenAdminWindow(UserSession session);
        void OpenEmployeeWindow(UserSession session);
        void Logout();
    }
}