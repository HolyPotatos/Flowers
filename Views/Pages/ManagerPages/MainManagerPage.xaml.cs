using Flowers.Model;
using System.Linq;
using System.Windows.Controls;

namespace Flowers.Views.Pages.ManagerPages
{
    public partial class MainManagerPage : Page
    {
        public MainManagerPage()
        {
            InitializeComponent();
            pppaCBox.ItemsSource = TradeEntities.GetContext().OrderPickupPoint.ToList();
        }

        private void GoOrderPageClick(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderManagerPage(((OrderPickupPoint)pppaCBox.SelectedItem).OrderPickupPointAddress));
        }
    }
}
