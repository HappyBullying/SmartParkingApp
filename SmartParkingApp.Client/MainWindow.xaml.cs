using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartParkingApp.Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            main_frame.Content = new Pages.RegistrationPage();
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
            Close();
        }
    }
}
