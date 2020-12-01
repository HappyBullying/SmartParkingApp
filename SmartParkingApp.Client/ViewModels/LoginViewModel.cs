using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Client.Commands;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace SmartParkingApp.Client.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
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
        private bool _isBtnRegisterEnabled = true;



        // Property for Name field
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");

                if (value.Length == 0)
                {
                    _nameReady = false;
                    UsernameColor = Colors.Red;
                }
                else
                {
                    _nameReady = true;
                    UsernameColor = Colors.Chocolate;
                }
                EnableLoginButton();
            }
        }
        private string _userName = "Name";



        // Property for Password field
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                if (value.Length == 0)
                {
                    _passwordReady = false;
                    PasswordColor = Colors.Red;
                }
                else
                {
                    _passwordReady = true;
                    PasswordColor = Colors.Chocolate;
                }
                EnableLoginButton();
            }
        }
        private string _password = "somepassword";


        private bool _nameReady = true;
        private bool _passwordReady = true;

        private void EnableLoginButton()
        {
            if (_nameReady && _passwordReady)
            {
                IsBtnRegisterEnabled = true;
            }
            else
            {
                IsBtnRegisterEnabled = false;
            }
        }
        // LoginCommand
        public LoginCommand Login_Command { get; private set; }
        public LoginCommand NavigateToRegister { get; private set; }
        /************************************************************************************/













        private readonly string _userRole;
        private ParkingManager _pkManager;
        private Action _navToMenue;
        public LoginViewModel(string userRole, ParkingManager pkm, Action navToMenue, Action navToRegister)
        {
            _userRole = userRole;
            _pkManager = pkm;
            Login_Command = new LoginCommand(Login);
            NavigateToRegister = new LoginCommand(navToRegister);
            _navToMenue = navToMenue;
        }

        private async void Login()
        {
            // Create new user object
            User usr = new User
            {
                Username = UserName,
                Password = Password
            };


            // Try to add user to the json database
            ResponseModel response = await _pkManager.Login(usr);
            
            if (!response.Succeded)
            {
                IssueWindow iss = new IssueWindow(response.Message);
                iss.ShowDialog();
            }
            else
            {
                _navToMenue?.Invoke();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
