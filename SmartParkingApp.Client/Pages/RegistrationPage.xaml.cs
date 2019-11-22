using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartParkingApp.Client.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            Assembly asm = GetType().GetTypeInfo().Assembly;
            Stream stream = asm.GetManifestResourceStream("SmartParkingApp.Client.Images.car_parking_ico.png");
            BitmapImage imgSource = new BitmapImage();
            imgSource.BeginInit();
            imgSource.StreamSource = stream;
            imgSource.EndInit();
            reg_logo_img.Source = imgSource;
            DataContext = new ViewModels.RegisterViewModel();
        }
    }
}
