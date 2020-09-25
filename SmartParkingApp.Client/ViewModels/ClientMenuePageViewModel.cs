using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;

namespace SmartParkingApp.Client.ViewModels
{
    public class ClientMenuePageViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        public ICommand ListButtonCommand { get; set; }
        public ICommand SessionButtonCommand { get; set; }
        public ICommand AccountButtnonCommand { get; set; }

        public ClientMenuePageViewModel(IRegionManager rM)
        {
            regionManager = rM;
            ListButtonCommand = new DelegateCommand(NavigateToCompletedOperations, () => true);
            SessionButtonCommand = new DelegateCommand(NavigateToCurrentSessionPage, () => true);
            AccountButtnonCommand = new DelegateCommand(NavigateToAccountPage, () => true);
        }


        private void NavigateToAccountPage()
        {
            regionManager.RequestNavigate("InnerContentRegion", "AccountPage");
        }


        private void NavigateToCompletedOperations()
        {
            regionManager.RequestNavigate("InnerContentRegion", "CompletedOperations");
        }


        private void NavigateToCurrentSessionPage()
        {
            regionManager.RequestNavigate("InnerContentRegion", "CurrentSessionPage");
        }
    }
}
