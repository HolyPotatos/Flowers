using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Flowers.Views.Pages
{
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
        private void GoBackClick(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Title = "Авторизация";
            NavigationService?.GoBack();
        }
    }
}
