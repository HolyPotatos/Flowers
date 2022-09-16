using Flowers.Views.Pages.AuthorizationPages;
using System.Windows;
using System.Windows.Input;

namespace Flowers.Views.Windows
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            AuthorizationFrame.Content = new AuthorizationPage();
        }
        private void DragWindowClick(object sender, MouseButtonEventArgs e) => DragMove();
        private void CloseApplicationClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void MinimizedApplicationClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
