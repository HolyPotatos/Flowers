using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Flowers.Views.Pages
{
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }
        private void CloseApplicationClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void PasswordPBoxPasswordChanged(object sender, RoutedEventArgs e) => PasswordPBox.SnapsToDevicePixels = PasswordPBox.Password.Length != 0 ? true : false;
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            ShowPasswordMaskTextBox.Clear();
            PasswordPBox.Clear();
            LoginTBox.Clear();
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Title = "Регистрация";
            NavigationService?.Navigate(new RegistrationPage());
        }
    }
}
