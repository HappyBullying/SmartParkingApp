using SmartParkingApp.ClassLibrary;
using System;
using System.Windows.Controls;

namespace SmartParkingApp.Client.Pages
{
    public partial class ClientMenuePage : Page
    {
        private int _userId;
        private Action _logoutAct;
        private ParkingManager _pk;

        public ClientMenuePage(Action logoutAct, ParkingManager pk)
        {
            InitializeComponent();
            _pk = pk;
            _logoutAct = logoutAct;
            ContentFrame.Content = new CompletedOperations(_userId, _pk);
        }

        private void ListButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is CompletedOperations))
            {
                ContentFrame.Content = new CompletedOperations(_userId, _pk);
            }
        }

        private void SessionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is CurrentSessionPage))
            {
                ContentFrame.Content = new CurrentSessionPage(_userId, _pk);
            }
        }

        private void AccountButtnon_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is AccountPage))
            {
                ContentFrame.Content = new AccountPage(_userId, _logoutAct, _pk);
            }
        }
    }
}
