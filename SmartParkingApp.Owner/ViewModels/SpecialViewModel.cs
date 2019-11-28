using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Owner.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SmartParkingApp.Owner.ViewModels
{
    class SpecialViewModel : INotifyPropertyChanged
    {
        // Properties for DataBinding
        /************************************************************************************/

        // Property for Since date
        public DateTime Since { get; set; }



        // Property for Until date
        public DateTime Until { get; set; }



        // Property for listview
        public List<KeyValuePair<int, double>> Statistics
        {
            get { return _Statistics; }
            private set
            {
                _Statistics = value;
                OnPropertyChanged("Statistics");
            }
        }
        private List<KeyValuePair<int, double>> _Statistics;

        // Command for calculating
        public SpecialCommand CalculateMaxQuantity { get; set; }
        /************************************************************************************/


        private int _userId;
        private ParkingManager _pk;
        public SpecialViewModel(int userId, ParkingManager pk)
        {
            _userId = userId;
            _pk = pk;
            CalculateMaxQuantity = new SpecialCommand(new Action(Calculate));
        }



        private void Calculate()
        {
            if ((Until - Since).TotalDays <= 0)
                return;

            List<ParkingSession> sessions = new List<ParkingSession>(_pk.GetSessionsInPeriod(_userId, Since, Until));
            TimeSpan ts = Until - Since;


            const int hr = 24;
            List<KeyValuePair<int, double>> result = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < hr; i++)
            {
                IEnumerable<ParkingSession> _s = from tmp in sessions
                                                 where
                            (tmp.EntryDt.Hour == i) ||
                            (tmp.ExitDt.Value.Hour == i)
                                                 select tmp;
                
                double rate = _s.Count() / ts.TotalDays;
                result.Add(new KeyValuePair<int, double>(i, rate));
            }

            Statistics = result;
        }

        private DateTime DtFloor(DateTime source)
        {
            return new DateTime(source.Year, source.Month, source.Day, source.Hour, 0, 0, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
