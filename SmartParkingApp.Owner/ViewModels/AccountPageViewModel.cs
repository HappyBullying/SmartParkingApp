using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Owner.Commands;
using System;
using System.ComponentModel;

namespace SmartParkingApp.Owner.ViewModels
{
    class AccountPageViewModel : INotifyPropertyChanged
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
                OnPropertyChanged("Name");
            }
        }
        private string _Name;



        // Property for CarPlateNumber
        public string UserRole
        {
            get { return _UserRole; }
            private set
            {
                _UserRole = value;
                OnPropertyChanged("UserRole");
            }
        }
        private string _UserRole;




        // Property for Phone
        public string Phone
        {
            get { return _Phone; }
            private set
            {
                _Phone = value;
                OnPropertyChanged("Phone");
            }
        }
        private string _Phone;



        // LogOut Command
        public AccountCommand LogOutCommand { get; set; }
        /************************************************************************************/






        private ParkingManager _pk;
        private int _userId;
        public AccountPageViewModel(int userId, Action logout, ParkingManager pk)
        {
            _pk = pk;
            _userId = userId;
            LogOutCommand = new AccountCommand(logout);
            User usr = _pk.CurrentUser;
            Name = usr.Username;
            Phone = usr.Phone;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
