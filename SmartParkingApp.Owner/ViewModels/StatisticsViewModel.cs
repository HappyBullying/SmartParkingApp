using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Owner.Commands;
using System;
using System.ComponentModel;

namespace SmartParkingApp.Owner.ViewModels
{
    class StatisticsViewModel : INotifyPropertyChanged
    {
        // Properties for DataBinding
        /************************************************************************************/
        // Property for Percent
        public double Percent
        {
            get { return _Percent; }
            private set
            {
                _Percent = value;
                OnPropertyChanged("Percent");
            }
        }
        private double _Percent = 0;
        public StatisticsCommand RefreshPercent { get; set; }
        /************************************************************************************/




        private ParkingManager _pk;
        private int _userId;

        public StatisticsViewModel(int userId, ParkingManager pk)
        {
            _pk = pk;
            _userId = userId;
            RefreshPercent = new StatisticsCommand(new Action(RefreshPercentMethod));
            RefreshPercentMethod();
        }


        private async void RefreshPercentMethod()
        {
            ResponseModel response = await _pk.GetPercentageofOccupiedSpace(_userId);
            Percent = (double)response.Data;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
