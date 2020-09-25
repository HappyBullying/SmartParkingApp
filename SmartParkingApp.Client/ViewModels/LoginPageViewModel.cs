using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media;

namespace SmartParkingApp.Client.Pages
{
    class LoginPageViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

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



        // Property for Password field
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value, "Password");
            }
        }
        private string _password = "somepassword";


        private bool LoginCanExecute()
        {
            bool cond1 = _userName.Length > 0;
            bool cond2 = _password.Length > 0;

            UsernameColor = cond1 ? Colors.Chocolate : Colors.Red;
            PasswordColor = cond2 ? Colors.Chocolate : Colors.Red;

            if (cond1 && cond2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // LoginCommand
        public DelegateCommand Login_Command { get; private set; }
        public DelegateCommand NavigateToRegister { get; private set; }
        /************************************************************************************/




        private readonly string _userRole;
        private ParkingManager _pkManager;
        public LoginPageViewModel(IRegionManager rM)
        {
            regionManager = rM;
            _userRole = UserRole.Client;
            _pkManager = StaticVars.manager;
            Login_Command = new DelegateCommand(Login, LoginCanExecute).
                ObservesProperty(() => Password).
                ObservesProperty(() => UserName);
            NavigateToRegister = new DelegateCommand(NavigateToRegisterImplim);
        }

        private void NavigateToRegisterImplim()
        {
            regionManager.RequestNavigate("ContentRegion", "RegistrationPage");
        }

        private void Login()
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
                UserRole = _userRole
            };


            // Try to add user to the json database
            string result = _pkManager.Login(usr);

            int userId;
            
            if (!int.TryParse(result, out userId))
            {
                IssueWindow iss = new IssueWindow(result);
                iss.ShowDialog();
            }
            else
            {
                regionManager.RequestNavigate("ContentRegion", "ClientMenuePage");
            }
        }
    }
}
