using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Owner.ViewModels;
using System.Windows.Controls;

namespace SmartParkingApp.Owner.Pages
{
    public partial class SpecialPage : Page
    {
        public SpecialPage(int userId, ParkingManager pk)
        {
            InitializeComponent();
            DataContext = new SpecialViewModel(userId, pk);
        }
    }
}
