using System.Windows;
using Flowers.Views.Pages.GuestPages;

namespace Flowers.Views.Windows
{
    public partial class BaseWindow : Window
    {
        public BaseWindow(int RoleUser)
        {
            InitializeComponent();
            this.Title = "Цветы: гость";
            BaseFrame.Content = new MainGuestPage();
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            var AuthWindow = new AuthorizationWindow();
            AuthWindow.Show();
            this.Close();
        }
    }
}
