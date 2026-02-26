using AIS.Warehouse.Data.Models;
using AIS.Warehouse.Logic;
using AIS.Warehouse.UI.Commands;
using AIS.Warehouse.UI.Models;
using AIS.Warehouse.UI.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace AIS.Warehouse.UI.ViewModels
{
    internal class LoginViewModel
    {
        private readonly INavigationService _navigation;
        private readonly UserRole _role;

        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public LoginViewModel(UserRole role, INavigationService navigation)
        {
            _role = role;
            _navigation = navigation;
            LoginCommand = new RelayCommand(_ => Login());
        }

        private void Login()
        {
            try
            {
                var auth = new AuthService();
                var session = auth.Login(Email, Password, _role);

                if (session == null || session.UserId == 0)
                {
                    MessageBox.Show("Неверный логин, пароль или роль");
                    return;
                }

                _navigation.OpenMainWindow(session);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка входа:\n" + ex.Message);
            }
        }
    }
}
