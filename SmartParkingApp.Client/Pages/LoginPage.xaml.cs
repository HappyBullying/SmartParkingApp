using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Client.ViewModels;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SmartParkingApp.Client.Pages
{
    public partial class LoginPage : Page
    {
        public LoginPage(ParkingManager pkm, Action navigateToMenue, Action navigateToRegister)
        {
            InitializeComponent();


            // ViewModel class
            LoginViewModel viewModelReg = new LoginViewModel(UserRole.Client, pkm, navigateToMenue, navigateToRegister);
            DataContext = viewModelReg;

            // Load image from resources
            Assembly asm = GetType().GetTypeInfo().Assembly;
            using (Stream stream = asm.GetManifestResourceStream("SmartParkingApp.Client.Images.car_parking_ico.png"))
            {
                BitmapImage imgSource = new BitmapImage();
                imgSource.BeginInit();
                imgSource.StreamSource = stream;
                imgSource.EndInit();
                reg_logo_img.Source = imgSource;
            }


            // PasswordBox Password property does not support binding
            passwdBox.PasswordChanged += (s, e) =>
            {
                viewModelReg.Password = passwdBox.Password;
            };
        }
    }
}
