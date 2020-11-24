using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Client.ViewModels;
using System;
using System.Windows.Controls;

namespace SmartParkingApp.Client.Pages
{
    public partial class CurrentSessionPage : Page
    {
        public CurrentSessionPage(int UserId, ParkingManager pkm)
        {
            InitializeComponent();
            DataContext = new CurrentSessionPageViewModel(UserId, pkm);
        }
    }
}
