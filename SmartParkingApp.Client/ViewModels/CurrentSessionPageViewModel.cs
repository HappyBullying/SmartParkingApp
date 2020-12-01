using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Client.Commands;
using System;
using System.ComponentModel;

namespace SmartParkingApp.Client.ViewModels
{
    class CurrentSessionPageViewModel : INotifyPropertyChanged
    {
        // Properties for DataBinding
        /************************************************************************************/
        // Property for CarPlateNumber
        public string CarPlateNumber
        {
            get { return _CarPlateNumber; }
            private set
            {
                _CarPlateNumber = value;
                OnPropertyChanged("CarPlateNumber");
            }
        }
        private string _CarPlateNumber;



        // Property for EntryDate
        public DateTime EntryDate
        {
            get { return _EntryDate; }
            private set
            {
                _EntryDate = value;
                OnPropertyChanged("EntryDate");
            }
        }
        private DateTime _EntryDate;




        // Property for CostAtThatMoment
        public decimal Cost
        {
            get { return _Cost; }
            private set
            {
                _Cost = value;
                OnPropertyChanged("Cost");
            }
        }
        private decimal _Cost;



        // Property for Enter button enable
        public bool EnterEnabled
        {
            get { return _EnterEnabled; }
            private set
            {
                _EnterEnabled = value;
                OnPropertyChanged("EnterEnabled");
            }
        }
        private bool _EnterEnabled = true;




        // Property for Pay button enable
        public bool PayEnabled
        {
            get { return _PayEnabled; }
            private set
            {
                _PayEnabled = value;
                OnPropertyChanged("PayEnabled");
            }
        }
        private bool _PayEnabled = false;




        // Property for Leave button enable
        public bool LeaveEnabled
        {
            get { return _LeaveEnable; }
            private set
            {
                _LeaveEnable = value;
                OnPropertyChanged("LeaveEnabled");
            }
        }
        private bool _LeaveEnable = false;




        // Property for Renew button enable
        public bool RenewEnabled
        {
            get { return _RenewEnabled; }
            private set
            {
                _RenewEnabled = value;
                OnPropertyChanged("RenewEnabled");
            }
        }
        private bool _RenewEnabled = false;


        // Commands
        public CurSesCommand EnterCommand { get; set; }
        public CurSesCommand PayCommand { get; set; }
        public CurSesCommand LeaveCommand { get; set; }
        public CurSesCommand RenewCommand { get; set; }
        /************************************************************************************/








        private ParkingManager _pk;
        private User _User;
        private ParkingSession _currentSession;
        private bool _payed = false;
        private IssueWindow issueWindow;

        public CurrentSessionPageViewModel(int UserId, ParkingManager pkm)
        {
            _pk = pkm;
            _User = _pk.CurrentUser;

            issueWindow = new IssueWindow("");
            EnterCommand = new CurSesCommand(new Action(StartParking));
            PayCommand = new CurSesCommand(new Action(Pay));
            RenewCommand = new CurSesCommand(new Action(RenewCost));
            LeaveCommand = new CurSesCommand(new Action(TryToLeave));
            LoadActiveIfExists();
        }






        // Перевести вGoogleBingif the user has closed the 
        // application it is necessary to download data from the 
        // active sessionif the user has closed the application it is 
        // necessary to download data from the active session
        private async void LoadActiveIfExists()
        {
            ResponseModel response1 = await _pk.GetActiveSessionForUser();
            _currentSession = (ParkingSession)response1.Data;
            
            if (_currentSession != null)
            {
                EnterEnabled = false;
                CarPlateNumber = _currentSession.CarPlateNumber;
                EntryDate = _currentSession.EntryDt;
                PayEnabled = true;
                RenewEnabled = true;
                ResponseModel response2 = await _pk.GetRemainingCost(_currentSession.TicketNumber);
                Cost = (decimal)response2.Data;
            }
        }


        private async void StartParking()
        {
            ResponseModel response = await _pk.EnterParking();
            _currentSession = (ParkingSession)response.Data;

            CarPlateNumber = _currentSession.CarPlateNumber;
            EntryDate = _currentSession.EntryDt;

            EnterEnabled = false;
            PayEnabled = true;
            RenewEnabled = true;
        }


        private async void TryToLeave()
        {
            bool leaveResult;
            ResponseModel response;
            if (_payed)
            {
                response = await _pk.TryLeaveParking(_currentSession.TicketNumber, _currentSession);
                leaveResult = response.Succeded;
            }
            else
            {
                response = await _pk.TryLeaveParkingByCarPlateNumber(CarPlateNumber, _currentSession);
                leaveResult = response.Succeded;
            }

            if (leaveResult)
            {
                _currentSession = null;
                EntryDate = default;
                Cost = 0;
                _payed = false;

                EnterEnabled = true;
                PayEnabled = false;
                LeaveEnabled = false;
                RenewEnabled = false;
            }
            else
            {
                if (!issueWindow.IsVisible)
                {
                    issueWindow.SetText("You can not leave the parking now");
                    issueWindow.ShowDialog();
                }
            }

        }


        private async void Pay()
        {
            ResponseModel response = await _pk.PayForParking(_currentSession.TicketNumber, CarPlateNumber);
            _payed = true;
            LeaveEnabled = true;
        }

        private async void RenewCost()
        {
            ResponseModel response = await _pk.GetRemainingCost(_currentSession.TicketNumber);
            Cost = Convert.ToDecimal(response.Data);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
