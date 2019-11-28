using System.Windows;

namespace SmartParkingApp.Owner
{
    public partial class IssueWindow : Window
    {
        public IssueWindow(string issueText)
        {
            InitializeComponent();
            IssueTextBlock.Text = issueText;
        }


        public void SetText(string text)
        {
            IssueTextBlock.Text = text;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DragWindow(object sender, RoutedEventArgs e)
        {
            DragMove();
        }
    }
}
