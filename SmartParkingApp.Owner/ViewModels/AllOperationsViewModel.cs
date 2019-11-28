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

            IEnumerable<ParkingSession> past = _pk.GetPastSesstionsForOwner(_userId);
            IEnumerable<ParkingSession> active = _pk.GetActiveSesstionsForOwner(_userId);

            PastSessions = new ObservableCollection<ParkingSession>(past);
            ActiveSessions = new ObservableCollection<ParkingSession>(active);

            List<ParkingSession> tmp = new List<ParkingSession>(past);
            tmp.AddRange(active);
            AllSessions = new ObservableCollection<ParkingSession>(tmp);
            Sessions = AllSessions;

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
