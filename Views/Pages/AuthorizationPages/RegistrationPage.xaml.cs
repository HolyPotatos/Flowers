using Flowers.Model;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Flowers.Views.Pages.AuthorizationPages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void GoBack()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Title = "Авторизация";
            NavigationService?.GoBack();
        }
        private void GoBackClick(object sender, RoutedEventArgs e) => GoBack();
        private void EmailTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "qwertyuiopasdfghjklzxc!#$%&'*+-/=?^_`{|}~vbnm1234567890@.".IndexOf(e.Text, StringComparison.OrdinalIgnoreCase) < 0;
        }

        private void RegistrationClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();
            if (string.IsNullOrEmpty(EmailTBox.Text)) errors.AppendLine("Введите адрес эл. почты");
            if (TradeEntities.GetContext().User.Any(b => b.UserLogin == EmailTBox.Text)) errors.AppendLine("Такой адрес эл. почты уже зарегистрирован");
            if (EmailTBox.Text[0] == '.' || EmailTBox.Text[EmailTBox.Text.Length-1] == '.') errors.AppendLine("Некорректный адрес эл. почты");
            if (string.IsNullOrEmpty(PasswordPBox.Password)) errors.AppendLine("Введите пароль");
            if (string.IsNullOrEmpty(RepeatPasswordPBox.Password)) errors.AppendLine("Повторите пароль");
            if (string.IsNullOrEmpty(NameTBox.Text)) errors.AppendLine("Введите имя");
            if (string.IsNullOrEmpty(SurnameTBox.Text)) errors.AppendLine("Введите фамилию");
            if (string.IsNullOrEmpty(PatronymicTBox.Text)) errors.AppendLine("Введите отчество");
            if (!(string.IsNullOrEmpty(RepeatPasswordPBox.Password) && string.IsNullOrEmpty(PasswordPBox.Password)) &&
                RepeatPasswordPBox.Password != PasswordPBox.Password) errors.AppendLine("Пароли не совпадают");
            if (errors.Length > 0)
                MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            try
            {
                var user = new User()
                {
                    UserLogin = EmailTBox.Text,
                    UserPassword = PasswordPBox.Password,
                    UserName = NameTBox.Text,
                    UserSurname = SurnameTBox.Text,
                    UserPatronymic = PatronymicTBox.Text,
                    UserRole = 3
                };
                TradeEntities.GetContext().User.Add(user);
                TradeEntities.GetContext().SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!", "Уведомление", MessageBoxButton.OK,
                    MessageBoxImage.Asterisk);
                GoBack();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
