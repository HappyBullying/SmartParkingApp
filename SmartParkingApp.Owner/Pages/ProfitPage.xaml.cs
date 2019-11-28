using SmartParkingApp.ClassLibrary;
using System.Windows.Controls;
using SmartParkingApp.Owner.ViewModels;

namespace SmartParkingApp.Owner.Pages
{
    public partial class ProfitPage : Page
    {
        public ProfitPage(int userId, ParkingManager pk)
        {
            InitializeComponent();
            DataContext = new ProfitViewModel(userId, pk);
        }
    }
}
