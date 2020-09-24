﻿using Prism.Mvvm;
using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SmartParkingApp.Client.ViewModels
{
    class CompleteOperationsViewModel : BindableBase
    {
        // Properties for DataBinding
        /************************************************************************************/
        public ObservableCollection<ParkingSession> Sessions { get; private set; }
        /************************************************************************************/


        private ParkingManager _pk;
        public CompleteOperationsViewModel(int userId, ParkingManager pk)
        {
            _pk = pk;
            IEnumerable<ParkingSession> received = _pk.GetCompletedSessionsForUser(userId);
            Sessions = new ObservableCollection<ParkingSession>(received);
            
        }
    }
}
