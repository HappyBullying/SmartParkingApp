using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Owner.ViewModels;
using System;
using System.Windows.Controls;

namespace SmartParkingApp.Owner.Pages
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
