using SmartParkingApp.ClassLibrary;
using System.Windows;
using Prism.Ioc;
using SmartParkingApp.Client.Pages;
using System;
using System.IO;
using System.Reflection;

namespace SmartParkingApp.Client
{
    public partial class App
    {

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            string DataPath;
            ParkingManager _pkManager;
            DataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SmartParkingApp\\Data";
            PrepareFiles(DataPath);
            _pkManager = new ParkingManager(DataPath);

            containerRegistry.Register(typeof(object), typeof(LoginPage), "LoginPage");
            containerRegistry.Register(typeof(object), typeof(RegistrationPage), "RegistrationPage");
            containerRegistry.Register(typeof(object), typeof(ClientMenuePage), "ClientMenuePage");
            containerRegistry.Register(typeof(object), typeof(CompletedOperations), "CompletedOperations");
            containerRegistry.Register(typeof(object), typeof(CurrentSessionPage), "CurrentSessionPage");
            containerRegistry.Register(typeof(object), typeof(AccountPage), "AccountPage");

            StaticVars.manager = _pkManager;
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);
            Current.MainWindow.Show();
        }

        // Prepares json files
        private void PrepareFiles(string DataPath)
        {
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }

            if (!File.Exists(DataPath + "\\Users.json"))
            {
                File.Create(DataPath + "\\Users.json").Close();
            }

            if (!File.Exists(DataPath + "\\ParkingData.json"))
            {

                using (Stream _res = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("SmartParkingApp.Client.Default.ParkingData.json"))
                {
                    using (FileStream file = new FileStream(DataPath + "\\ParkingData.json", FileMode.Create))
                    {
                        _res.CopyTo(file);
                    }
                }

            }

            if (!File.Exists(DataPath + "\\Tariffs.json"))
            {
                using (Stream _res = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("SmartParkingApp.Client.Default.Tariffs.json"))
                {
                    using (FileStream file = new FileStream(DataPath + "\\Tariffs.json", FileMode.Create))
                    {
                        _res.CopyTo(file);
                    }
                }
            }
        }
    }
}
