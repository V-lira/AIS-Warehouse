using AIS.Warehouse.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AIS.Warehouse.UI.ViewModels;

namespace AIS.Warehouse.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для AddOperationWindow.xaml
    /// </summary>
    public partial class AddOperationWindow : Window
    {
        public AddOperationWindow(
            OperationRepository operationRepo,
            ItemRepository itemRepo)
        {
            InitializeComponent();
            var vm = new AddOperationViewModel2(operationRepo, itemRepo);
            vm.Window = this;
            DataContext = vm;
        }
    }
}
