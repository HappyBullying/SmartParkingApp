using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SmartParkingApp.Client.ViewModels
{
    class RegisterViewModel : INotifyPropertyChanged
    {
        
        // Properties for DataBinding
        /************************************************************************************/
        // Color for border relative to username field
        public Color UsernameColor
        {
            get { return _usernameColor; }
            private set
            {
                _usernameColor = value;
                OnPropertyChanged("UsernameColor");
            }
        }
        private Color _usernameColor = Colors.Chocolate;



        // Color for border relative to password field
        public Color PasswordColor
        {
            get { return _passwordColor; }
            private set
            {
                _passwordColor = value;
                OnPropertyChanged("PasswordColor");
            }
        }
        private Color _passwordColor = Colors.Chocolate;



        // Color for border relative to CarPlateNumber field
        public Color CarPlateNumberColor
        {
            get { return _carPlateNumberColor; }
            private set
            {
                _carPlateNumberColor = value;
                OnPropertyChanged("CarPlateNumber");
            }
        }
        private Color _carPlateNumberColor = Colors.Chocolate;



        // Color for border relative to PhoneNumber
        public Color PhoneNumberColor
        {
            get { return _phoneNumberColor; }
            private set
            {
                _phoneNumberColor = value;
                OnPropertyChanged("PhoneNumber");
            }
        }
        private Color _phoneNumberColor = Colors.Chocolate;



        // Property for button enable
        public bool IsBtnRegisterEnabled
        {
            get { return _isBtnRegisterEnabled; }
            private set
            {
                _isBtnRegisterEnabled = value;
                OnPropertyChanged("IsBtnRegisterEnabled");
            }
        }
        private bool _isBtnRegisterEnabled = false;


        // Property for Name field
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string _userName = "Name";


        
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private string _password = "123";



        // Property for CarPlateNumber
        public string CarPlateNumber
        {
            get { return _carPlateNumber; }
            set
            {
                _carPlateNumber = value;
                OnPropertyChanged("CarPlateNumber");
            }
        }
        private string _carPlateNumber = "CarPlateNumber";




        // Property for PhoneNumber
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }
        private string _phoneNumber = "PhoneNumber";
        /************************************************************************************/




        public RegisterViewModel()
        {

        }

        
        public event PropertyChangedEventHandler PropertyChanged;



        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
