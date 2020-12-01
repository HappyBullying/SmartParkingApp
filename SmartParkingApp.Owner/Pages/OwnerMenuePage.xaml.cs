using SmartParkingApp.ClassLibrary;
using System;
using System.Windows.Controls;

namespace SmartParkingApp.Owner.Pages
{
    public partial class OwnerMenuePage : Page
    {
        private int _userId;
        private Action _logoutAct;
        private ParkingManager _pk;

        public OwnerMenuePage(Action logoutAct, ParkingManager pk)
        {
            InitializeComponent();
            _pk = pk;
            _logoutAct = logoutAct;
            ContentFrame.Content = new AllOperationsPage(_userId, _pk);
        }

        private void ListButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is AllOperationsPage))
            {
                ContentFrame.Content = new AllOperationsPage(_userId, _pk);
            }
        }

        private void PercentButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is StatisticsPage))
            {
                ContentFrame.Content = new StatisticsPage(_userId, _pk);
            }
        }

        private void AccountButtnon_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is AccountPage))
            {
                ContentFrame.Content = new AccountPage(_userId, _logoutAct, _pk);
            }
        }

        private void ProfitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is ProfitPage))
            {
                ContentFrame.Content = new ProfitPage(_userId, _pk);
            }
        }

        private void SpecialButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!(ContentFrame.Content is SpecialPage))
            {
                ContentFrame.Content = new SpecialPage(_userId, _pk);
            }
        }
    }
}
