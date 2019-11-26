using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Client.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingApp.Client.ViewModels
{
    class CompleteOpeartionsViewModel : INotifyPropertyChanged
    {
        // Properties for DataBinding
        /************************************************************************************/
        public ObservableCollection<ParkingSession> Sessions { get; private set; }
        /************************************************************************************/













        public CompleteOpeartionsViewModel()
        {

            Sessions = new ObservableCollection<ParkingSession>()
            {
                new ParkingSession
                {
                    CarPlateNumber = "wadawadawadawadawadawadawadawadawadawadawadawadawadawadawadawadawadawadawd",
                    EntryDt = DateTime.Now,
                    PaymentDt = DateTime.Now,
                    ExitDt = DateTime.Now,
                    TicketNumber = 1,
                     TotalPayment = 3,
                      UserId = 1
                },
                new ParkingSession
                {
                    CarPlateNumber = "aaaaa",
                    EntryDt = DateTime.Now,
                    PaymentDt = DateTime.Now,
                    ExitDt = DateTime.Now,
                    TicketNumber = 2,
                     TotalPayment = 1111,
                      UserId = 23
                } };
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
