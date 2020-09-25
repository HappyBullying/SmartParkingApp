using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using System;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Regions;
using Prism.Ioc;
using System.Windows.Input;

namespace SmartParkingApp.Client.ViewModels
{
    class AccountPageViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

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
        public ICommand LogOutCommand { get; set; }
        public bool CanExecuteLogOut()
        {
            return true;
        }

        private void LogOut()
        {
            regionManager.RequestNavigate("ContentRegion", "LoginPage");
        }

        /************************************************************************************/






        private ParkingManager _pk;
        public AccountPageViewModel(IRegionManager rM)
        {
            _pk = StaticVars.manager;
            LogOutCommand = new DelegateCommand(LogOut, CanExecuteLogOut);
            User usr = _pk.GetUserById(StaticVars.TransferID);
            Name = usr.Name;
            CarPlateNumber = usr.CarPlateNumber;
            Phone = usr.Phone;
        }
    }
}
