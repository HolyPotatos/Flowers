using Flowers.Model.Classes;
using Flowers.Views.Pages.AdminPages;
using Flowers.Views.Pages.GuestPages;
using Flowers.Views.Pages.ManagerPages;
using Flowers.Views.Pages.UserPages;
using System.Linq;
using System.Windows;

namespace Flowers.Views.Windows
{
    public partial class BaseWindow : Window
    {
        public BaseWindow(int RoleUser)
        {
            InitializeComponent();
            if (RoleUser!=0)
            {
                TitleTBox.Text =
                    TradeEntities.GetContext().User.First(b => b.UserID == UserIDClass.UID).UserSurname + " " +
                    TradeEntities.GetContext().User.First(b => b.UserID == UserIDClass.UID).UserName + " " +
                    TradeEntities.GetContext().User.First(b => b.UserID == UserIDClass.UID).UserPatronymic;
            }
            switch (RoleUser)
            {
                case 0:
                    BaseFrame.Content = new MainGuestPage();
                    TitleTBox.Text = "Неавторизованный пользователь";
                    this.Title = "Цветы: гость";
                    break;
                case 1:
                    BaseFrame.Content = new MainAdminPage();
                    this.Title = "Цветы: администратор";
                    break;
                case 2:
                    BaseFrame.Content = new MainManagerPage();
                    this.Title = "Цветы: менеджер";
                    break;
                case 3:
                    BaseFrame.Content = new MainUserPage();
                    this.Title = "Цветы: пользователь";
                    break;
            }
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            UserIDClass.UID = -1;
            var AuthWindow = new AuthorizationWindow();
            AuthWindow.Show();
            this.Close();
        }
    }
}
