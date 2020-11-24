using System;
using System.Windows.Input;

namespace SmartParkingApp.Client.Commands
{
    class CurSesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _action;
        public CurSesCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke();
        }
    }
}
