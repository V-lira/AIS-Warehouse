using AIS.Warehouse.Data.Models;
using AIS.Warehouse.UI.Models;
using AIS.Warehouse.UI.ViewModels;
using AIS.Warehouse.UI.Views;
using System.Linq;
using System.Windows;

namespace AIS.Warehouse.UI.Services
{
    public class NavigationService : INavigationService
    {
        public void OpenLogin(UserRole role)
        {
            var loginWindow = new LoginWindow(role, this);
            loginWindow.Show();

            CloseRoleSelectionWindow();
        }

        public void OpenMainWindow(UserSession session)
        {
            if (session.Role == UserRole.Admin)
                OpenAdminWindow(session);
            else
                OpenEmployeeWindow(session);
        }

        public void OpenAdminWindow(UserSession session)
        {
            var adminWindow = new AdminWindow
            {
                DataContext = new AdminWindowViewModel2(session, this)
            };

            adminWindow.Show();
            CloseLoginWindows();
        }

        public void OpenEmployeeWindow(UserSession session)
        {
            var mainWindow = new _1С
            {
                DataContext = new MainViewModel(session, this)
            };

            mainWindow.Show();
            CloseLoginWindows();
        }

        public void Logout()
        {
            foreach (Window window in Application.Current.Windows)
                window.Close();

            var roleWindow = new RoleSelectionWindow
            {
                DataContext = new RoleSelectionViewModel(this)
            };
            roleWindow.Show();
        }

        private void CloseLoginWindows()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginWindow)
                    window.Close();
            }
        }

        private void CloseRoleSelectionWindow()
        {
            Application.Current.Windows
                .OfType<RoleSelectionWindow>()
                .FirstOrDefault()
                ?.Close();
        }
    }
}
