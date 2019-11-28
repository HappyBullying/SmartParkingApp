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
            _User = _pk.GetUserById(UserId);

            issueWindow = new IssueWindow("");
            EnterCommand = new CurSesCommand(new Action(StartParking));
            PayCommand = new CurSesCommand(new Action(Pay));
            RenewCommand = new CurSesCommand(new Action(RenewCost));
            LeaveCommand = new CurSesCommand(new Action(TryToLeave));
        }


        private void StartParking()
        {
            _currentSession = _pk.EnterParking(_User.CarPlateNumber);


            CarPlateNumber = _currentSession.CarPlateNumber;
            EntryDate = _currentSession.EntryDt;



            EnterEnabled = false;
            PayEnabled = true;
            RenewEnabled = true;
        }


        private void TryToLeave()
        {
            bool leaveResult; 

            if (_payed)
            {
                leaveResult = _pk.TryLeaveParkingWithTicket(_currentSession.TicketNumber, _currentSession);
            }
            else
            {
                leaveResult = _pk.TryLeaveParkingByCarPlateNumber(CarPlateNumber, _currentSession);
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
                    issueWindow.Show();
                }
            }

        }


        private void Pay()
        {
            decimal amount = _pk.GetRemainingCost(_currentSession.TicketNumber);
            _pk.PayForParking(_currentSession.TicketNumber, amount);
            _payed = true;
            LeaveEnabled = true;
        }

        private void RenewCost()
        {
            Cost = _pk.GetRemainingCost(_currentSession.TicketNumber);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
