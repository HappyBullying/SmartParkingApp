using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Owner.ViewModels;
using System.Windows.Controls;

namespace SmartParkingApp.Owner.Pages
{
    public partial class StatisticsPage : Page
    {
        public StatisticsPage(int userId, ParkingManager pk)
        {
            InitializeComponent();
            DataContext = new StatisticsViewModel(userId, pk);
        }
    }
}
