using SmartParkingApp.ClassLibrary;
using SmartParkingApp.Client.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System;
using System.Reflection;

namespace SmartParkingApp.Client
{
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            

            // Disable navigation bar in frame
            main_frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        










        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragEnter += (s, t) =>
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
            };
            if (e.ClickCount == 1)
            {
                DragMove();
            }
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                    WindowMaximize.Content = Resources["WMaximize"];
                }
                else
                {
                    WindowState = WindowState.Maximized;
                    WindowMaximize.Content = Resources["WRestore"];
                }
            }
            
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void WindowMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                (sender as Button).Content = Resources["WRestore"];
                
            }
            else
            {
                WindowState = WindowState.Normal;
                (sender as Button).Content = Resources["WMaximize"];
            }
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
