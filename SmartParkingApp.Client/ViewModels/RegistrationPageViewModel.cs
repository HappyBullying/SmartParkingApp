using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartParkingApp.Client.ViewModels
{
    class RegistrationPageViewModel : BindableBase
    {

        // Properties for DataBinding
        /************************************************************************************/
        // Color for border relative to username field
        public Color UsernameColor
        {
            get { return _usernameColor; }
            private set
            {
                SetProperty(ref _usernameColor, value, "UsernameColor");
            }
        }
        private Color _usernameColor = Colors.Chocolate;



        // Color for border relative to password field
        public Color PasswordColor
        {
            get { return _passwordColor; }
            private set
            {
                SetProperty(ref _passwordColor, value, "PasswordColor");
            }
        }
        private Color _passwordColor = Colors.Chocolate;



        // Color for border relative to CarPlateNumber field
        public Color CarPlateNumberColor
        {
            get { return _carPlateNumberColor; }
            private set
            {
                SetProperty(ref _carPlateNumberColor, value, "CarPlateNumber");
            }
        }
        private Color _carPlateNumberColor = Colors.Chocolate;



        // Color for border relative to PhoneNumber
        public Color PhoneNumberColor
        {
            get { return _phoneNumberColor; }
            private set
            {
                SetProperty(ref _phoneNumberColor, value, "PhoneNumberColor");
            }
        }
        private Color _phoneNumberColor = Colors.Chocolate;



        // Property for Name field
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value, "UserName");
            }
        }
        private string _userName = "Name";


        // Property for password
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value, "Password");
            }
        }
        private string _password = "";



        // Property for CarPlateNumber
        public string CarPlateNumber
        {
            get { return _carPlateNumber; }
            set
            {
                SetProperty(ref _carPlateNumber, value, "CarPlateNumber");
            }
        }
        private string _carPlateNumber = "CarPlateNumber";




        // Property for PhoneNumber
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                SetProperty(ref _phoneNumber, value, "PhoneNumber");
            }
        }
        private string _phoneNumber = "PhoneNumber";


        // Register Command
        public DelegateCommand RegisterUserCommand { get; private set; }
        public ICommand NavigateToLogin { get; private set; }

        /************************************************************************************/









        private readonly string _userRole;
        private ParkingManager _pkManager;
        public RegistrationPageViewModel(IRegionManager rM)
        {
            _userRole = UserRole.Client;
            _pkManager = StaticVars.manager;
            RegisterUserCommand = new DelegateCommand(Register, RegisterCanExecute).
                ObservesProperty(() => UserName).
                ObservesProperty(() => Password).
                ObservesProperty(() => CarPlateNumber).
                ObservesProperty(() => PhoneNumber);
            NavigateToLogin = new DelegateCommand(() =>
            {
                rM.RequestNavigate("ContentRegion", "LoginPage");
            }, () => true);
        }


        private void Register()
        {
            // Compute MD5 hash for password
            MD5 md5 = MD5.Create();
            byte[] input = Encoding.ASCII.GetBytes(Password);
            byte[] hash = md5.ComputeHash(input);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                // Add letter into full hash string uppercase
                sb.Append(hash[i].ToString("X2"));
            }

            // Create new user object
            User usr = new User
            {
                Name = UserName,
                PasswordHash = sb.ToString(),
                CarPlateNumber = CarPlateNumber,
                Phone = PhoneNumber,
                UserRole = _userRole
            };


            // Try to add user to the json database
            string result = _pkManager.RegisterNewUser(usr);
            if (!result.Equals("Successfully"))
            {
                IssueWindow iss = new IssueWindow(result);
                iss.ShowDialog();
            }
            else
            {
                if (NavigateToLogin.CanExecute(result))
                {
                    NavigateToLogin.Execute(null);
                }
            }
        }

        private bool RegisterCanExecute()
        {
            bool uNameCond;
            Match m = Regex.Match(_userName, "^[A-Za-z]*[A-Za-z]+[A-Za-z0-9_]*$");
            if (m.Success)
            {
                UsernameColor = Colors.Chocolate;
                uNameCond = true;
            }
            else
            {
                UsernameColor = Colors.Red;
                uNameCond = false;
            }

            bool passwordCond;
            m = Regex.Match(_password, @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,35}$");
            if (m.Success)
            {
                PasswordColor = Colors.Chocolate;
                passwordCond = true;
            }
            else
            {
                PasswordColor = Colors.Red;
                passwordCond = false;
            }



            bool carPlateCond;
            if (_carPlateNumber.Length == 0)
            {
                _carPlateNumberColor = Colors.Red;
                carPlateCond = false;
            }
            else
            {
                _carPlateNumberColor = Colors.Chocolate;
                carPlateCond = true;
            }


            bool phoneCond;
            m = Regex.Match(_phoneNumber, @"^((\+[0-9])+([0-9]){10,16})$");

            if (m.Success)
            {
                PhoneNumberColor = Colors.Chocolate;
                phoneCond = true;
            }
            else
            {
                PhoneNumberColor = Colors.Red;
                phoneCond = false;
            }

            if (uNameCond && passwordCond && carPlateCond && phoneCond)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
