using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using System;
using Prism.Mvvm;
using Prism.Commands;

namespace SmartParkingApp.Client.ViewModels
{
    class AccountPageViewModel : BindableBase
    {
        // Properties for DataBinding
        /************************************************************************************/
        // Property for Name
        public string Name
        {
            get { return _Name; }
            private set
            {
                _Name = value;
                SetProperty(ref _Name, value, "Name");
            }
        }
        private string _Name;



        // Property for CarPlateNumber
        public string CarPlateNumber
        {
            get { return _CarPlateNumber; }
            private set
            {
                _CarPlateNumber = value;
                SetProperty(ref _CarPlateNumber, value, "CarPlateNumber");
            }
        }
        private string _CarPlateNumber;




        // Property for Phone
        public string Phone
        {
            get { return _Phone; }
            private set
            {
                _Phone = value;
                SetProperty(ref _Phone, value, "Phone");
            }
        }
        private string _Phone;



        // LogOut Command
        public DelegateCommand LogOutCommand { get; set; }
        private Action logOutAction;
        public bool CanExecute()
        {
            return true;
        }

        public void Execute()
        {
            logOutAction?.Invoke();
        }
        /************************************************************************************/






        private ParkingManager _pk;
        public AccountPageViewModel(int userId, Action logout, ParkingManager pk)
        {
            _pk = pk;
            logOutAction = logout;
            LogOutCommand = new DelegateCommand(Execute, CanExecute);
            User usr = _pk.GetUserById(userId);
            Name = usr.Name;
            CarPlateNumber = usr.CarPlateNumber;
            Phone = usr.Phone;
        }
    }
}
