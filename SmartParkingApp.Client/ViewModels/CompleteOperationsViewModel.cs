using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Client.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartParkingApp.Client.ViewModels
{
    class CompleteOperationsViewModel : INotifyPropertyChanged
    {
        // Properties for DataBinding
        /************************************************************************************/
        public ObservableCollection<ParkingSession> Sessions { get; private set; }
        /************************************************************************************/

        public ICommand RenewCommand { get; set; }



        private ParkingManager _pk;
        Action<List<ParkingSession>> _renewAction;
        public CompleteOperationsViewModel(int userId, ParkingManager pk, Action<List<ParkingSession>> renewAction)
        {
            _pk = pk;
            RenewCommand = new CompleteOperationCommand(GetCompleted);
            //GetCompleted(userId, received);
            _renewAction = renewAction;
            //Sessions = new ObservableCollection<ParkingSession>(received);
            
        }

        private async void GetCompleted()
        {
            ResponseModel response = await _pk.GetCompletedSessionsForUser();
            _renewAction.Invoke((List<ParkingSession>)response.Data);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
