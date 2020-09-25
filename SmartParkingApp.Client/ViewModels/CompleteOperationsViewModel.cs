using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using SmartParkingApp.ClassLibrary;
using SmartParkingApp.ClassLibrary.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartParkingApp.Client.ViewModels
{
    class CompleteOperationsViewModel : BindableBase
    {
        // Properties for DataBinding
        /************************************************************************************/
        public ObservableCollection<ParkingSession> Sessions { get; private set; }
        /************************************************************************************/


        private ParkingManager _pk;
        public CompleteOperationsViewModel(IRegionManager rM)
        {
            _pk = StaticVars.manager;
            IEnumerable<ParkingSession> received = _pk.GetCompletedSessionsForUser(StaticVars.TransferID);
            Sessions = new ObservableCollection<ParkingSession>(received);
            
        }
    }
}
