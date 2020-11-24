using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Client.ViewModels;
using System;
using System.Windows.Controls;

namespace SmartParkingApp.Client.Pages
{
    public partial class AccountPage : Page
    {
        public AccountPage(int userId, Action _logoutAction, ParkingManager pk)
        {
            InitializeComponent();
            DataContext = new AccountPageViewModel(userId, _logoutAction, pk);
        }
    }
}
