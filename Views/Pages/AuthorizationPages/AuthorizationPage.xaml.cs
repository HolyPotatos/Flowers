using Flowers.Model.Classes;
using Flowers.Views.Windows;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Flowers.Views.Pages.AuthorizationPages
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
            var errors = new StringBuilder();
            if (string.IsNullOrEmpty(LoginTBox.Text))
                errors.AppendLine("Введите логин");
            if (string.IsNullOrEmpty(PasswordPBox.Password))
                errors.AppendLine("Введите пароль");
            if (errors.Length != 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var RoleUser = CheckUserRoleClass.CheckUserRole(LoginTBox.Text, PasswordPBox.Password);
            if (RoleUser == 0)
                MessageBox.Show("Такого аккаунта нет!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                var BaseWindow = new BaseWindow(RoleUser);
                BaseWindow.Show();
                window.Close();
            }

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

        private void GuestEnterClick(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            var BaseWindow = new BaseWindow(0);
            BaseWindow.Show();
            window.Close();
        }
    }
}
