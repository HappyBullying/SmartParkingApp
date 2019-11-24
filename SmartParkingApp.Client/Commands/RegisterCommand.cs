using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartParkingApp.Client.Commands
{
    class RegisterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;
        public RegisterCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object param)
        {
            return true;
        }

        public void Execute(object param)
        {
            _execute?.Invoke();
        }
    }
}
