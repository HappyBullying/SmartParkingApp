using SmartParkingApp.ClassLibrary.Models;
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
        public ObservableCollection<ParkingSession> Sessions { get; set; }


        public CompleteOpeartionsViewModel()
        {
            Sessions = new ObservableCollection<ParkingSession>()
            {
                new ParkingSession
                {
                    CarPlateNumber = "wadawd",
                    EntryDt = DateTime.Now,
                    PaymentDt = DateTime.Now,
                    ExitDt = DateTime.Now,
                    TicketNumber = 1,
                     TotalPayment = 3,
                      UserId = 1
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
