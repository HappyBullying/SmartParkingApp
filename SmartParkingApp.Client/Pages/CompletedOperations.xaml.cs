using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Client.ViewModels;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SmartParkingApp.Client.Pages
{
    public partial class CompletedOperations : Page
    {
        public CompletedOperations(int userId, ParkingManager pk)
        {
            InitializeComponent();
            DataContext = new CompleteOperationsViewModel(userId, pk, Renew);
            //ParkingSession.ItemsSource = (DataContext as CompleteOperationsViewModel).Sessions;
        }

        private void Renew(List<ParkingSession> sessions)
        {
            this.ParkingSession.ItemsSource = sessions;
        }
    }
}
