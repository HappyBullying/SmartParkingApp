using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Owner.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace SmartParkingApp.Owner.ViewModels
{
    class AllOperationsViewModel : INotifyPropertyChanged
    {
        // Properties for DataBinding
        /************************************************************************************/
        

         // Collection to display
        public ObservableCollection<ParkingSession> Sessions { get; set; }




        // SelectedItemValue
        public string Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                SetNewListMthod();
            }
        }
        private string _Selected;


        private ObservableCollection<ParkingSession> PastSessions;
        private ObservableCollection<ParkingSession> ActiveSessions;
        private ObservableCollection<ParkingSession> AllSessions;


        /************************************************************************************/





        private ParkingManager _pk;
        private int _userId;
        public AllOperationsViewModel(int userId, ParkingManager pk)
        {
            _pk = pk;
            _userId = userId;

            List<ParkingSession> past = null; 
            List<ParkingSession> active = null;
            GetSessions(active, past);
            PastSessions = new ObservableCollection<ParkingSession>(past);
            ActiveSessions = new ObservableCollection<ParkingSession>(active);

            List<ParkingSession> tmp = new List<ParkingSession>(past);
            tmp.AddRange(active);
            AllSessions = new ObservableCollection<ParkingSession>(tmp);
            Sessions = AllSessions;

        }

        private async void GetSessions(List<ParkingSession> active, List<ParkingSession> past)
        {
            ResponseModel responseActive = await _pk.GetPastSesstionsForOwner(_userId);
            ResponseModel responsePast = await _pk.GetActiveSesstionsForOwner(_userId);

            active = (List<ParkingSession>)responseActive.Data;
            past = (List<ParkingSession>)responsePast.Data;
        }


        private void SetNewListMthod()
        {
            switch(Selected)
            {
                case "PastSessions":
                    {
                        Sessions = PastSessions;
                        break;
                    }
                case "ActiveSessions":
                    {
                        Sessions = ActiveSessions;
                        break;
                    }
                case "All":
                    {
                        Sessions = AllSessions;
                        break;
                    }
                default:
                    break;
            }
            OnPropertyChanged("Sessions");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
