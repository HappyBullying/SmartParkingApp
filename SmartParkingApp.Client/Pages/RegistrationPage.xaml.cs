using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Client.ViewModels;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SmartParkingApp.Client.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage(ParkingManager pkm, Action navigateToLogin)
        {
            InitializeComponent();

            // ViewModel class
            RegisterViewModel viewModelReg = new RegisterViewModel(UserRole.Client, pkm, navigateToLogin);
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
