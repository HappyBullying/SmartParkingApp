using Ninject;
using SmartParkingApp.ClassLibrary;
using System.Windows;

namespace SmartParkingApp.Client
{
    public partial class App : Application
    {
        public App()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<ParkingManager>().To<ParkingManager>();
        }
    }
}
