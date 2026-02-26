using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//данный класс это мост между UI и бизнес-логики.
//пользователь -> клик -> выполнить вот этот метод
namespace AIS.Warehouse.UI.Commands
{
    //ICommand это некий "контракт" wpf
    //прикол в том, что wpf сам перепроверяет состояние команды, когда:
    //пользователь что-то вводит. фокус меняется или другие события.
    public class RelayCommand : ICommand
    {
        //что выполнить:
        private readonly Action<object> _execute;
        //когда можно выполнить:
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        //кнопка работает? да или нет?
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        //по клику клик и вызвалась, запускает метод проосто.
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        //тут меняется состояние через механизм wpf (который встроен)
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}