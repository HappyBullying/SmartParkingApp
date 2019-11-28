using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Client.ViewModels;
using System.Windows.Controls;

namespace SmartParkingApp.Client.Pages
{
    public partial class CompletedOperations : Page
    {
        public CompletedOperations(int userId, ParkingManager pk)
        {
            InitializeComponent();
            DataContext = new CompleteOperationsViewModel(userId, pk);
            ParkingSession.ItemsSource = (DataContext as CompleteOperationsViewModel).Sessions;
            
        }
    }
}
