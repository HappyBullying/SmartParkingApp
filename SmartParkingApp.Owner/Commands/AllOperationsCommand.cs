using System;
using System.Windows.Input;

namespace SmartParkingApp.Owner.Commands
{
    class AllOperationsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _action;


        public AllOperationsCommand(Action action)
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
