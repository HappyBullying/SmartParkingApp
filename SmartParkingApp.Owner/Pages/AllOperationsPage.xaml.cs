using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Owner.ViewModels;
using System.Windows.Controls;

namespace SmartParkingApp.Owner.Pages
{
    public partial class AllOperationsPage : Page
    {
        public AllOperationsPage(int userId, ParkingManager pk)
        {
            InitializeComponent();
            DataContext = new AllOperationsViewModel(userId, pk);
            //ParkingSession.ItemsSource = (DataContext as AllOperationsViewModel).Sessions;
        }
    }
}
