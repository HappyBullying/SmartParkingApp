using Prism.Commands;
using Prism.Mvvm;
using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using SmartParkingApp.Client.Commands;
using System;
using System.ComponentModel;

namespace SmartParkingApp.Client.ViewModels
{
    class CurrentSessionPageViewModel : BindableBase
    {
        // Properties for DataBinding
        /************************************************************************************/
        // Property for CarPlateNumber
        public string CarPlateNumber
        {
            get { return _CarPlateNumber; }
            private set
            {
                SetProperty(ref _CarPlateNumber, value, "CarPlateNumber");
            }
        }
        private string _CarPlateNumber;



        // Property for EntryDate
        public DateTime EntryDate
        {
            get { return _EntryDate; }
            private set
            {
                SetProperty(ref _EntryDate, value, "EntryDate");
            }
        }
        private DateTime _EntryDate;




        // Property for CostAtThatMoment
        public decimal Cost
        {
            get { return _Cost; }
            private set
            {
                SetProperty(ref _Cost, value, "Cost");
            }
        }
        private decimal _Cost;



        // Property for Enter button enable
        public bool EnterEnabled
        {
            get { return _EnterEnabled; }
            private set
            {
                SetProperty(ref _EnterEnabled, value, "EnterEnabled");
            }
        }
        private bool _EnterEnabled = true;




        // Property for Pay button enable
        public bool PayEnabled
        {
            get { return _PayEnabled; }
            private set
            {
                SetProperty(ref _PayEnabled, value, "PayEnabled");
            }
        }
        private bool _PayEnabled = false;




        // Property for Leave button enable
        public bool LeaveEnabled
        {
            get { return _LeaveEnable; }
            private set
            {
                SetProperty(ref _LeaveEnable, value, "LeaveEnabled");
            }
        }
        private bool _LeaveEnable = false;




        // Property for Renew button enable
        public bool RenewEnabled
        {
            get { return _RenewEnabled; }
            private set
            {
                SetProperty(ref _RenewEnabled, value, "RenewEnabled");
            }
        }
        private bool _RenewEnabled = false;


        // Commands
        public DelegateCommand EnterCommand { get; set; }
        public DelegateCommand PayCommand { get; set; }
        public DelegateCommand LeaveCommand { get; set; }
        public DelegateCommand RenewCommand { get; set; }
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
            EnterCommand = new DelegateCommand(StartParking).
                ObservesProperty(() => EnterEnabled);
            PayCommand = new DelegateCommand(Pay).
                ObservesProperty(() => PayEnabled);
            RenewCommand = new DelegateCommand(RenewCost, CanExecuteRenew).
                ObservesProperty(() => RenewEnabled);
            LeaveCommand = new DelegateCommand(TryToLeave).
                ObservesProperty(() => LeaveEnabled);
            LoadActiveIfExists();
        }






        // application it is necessary to download data from the 
        // active sessionif the user has closed the application it is 
        // necessary to download data from the active session
        private void LoadActiveIfExists()
        {
            _currentSession = _pk.GetActiveSessionForUser(_User.Id);
            
            if (_currentSession != null)
            {
                EnterEnabled = false;
                CarPlateNumber = _currentSession.CarPlateNumber;
                EntryDate = _currentSession.EntryDt;
                PayEnabled = true;
                RenewEnabled = true;
                Cost = _pk.GetRemainingCost(_currentSession.TicketNumber);
            }
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
                    issueWindow.ShowDialog();
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

        private bool CanExecuteRenew()
        {
            if (_currentSession == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
