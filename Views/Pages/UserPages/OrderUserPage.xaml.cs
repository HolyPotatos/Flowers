using Flowers.Model;
using Flowers.Model.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Flowers.Views.Pages.UserPages
{
    public partial class OrderUserPage : Page
    {
        private void FilterOrder(string TextFilter)
        {
            OrderDataGrid.ItemsSource = null;

            if (string.IsNullOrEmpty(TextFilter))
            {

                OrderDataGrid.ItemsSource = TradeEntities.GetContext().Order.Where(b => b.User.UserID == UserIDClass.UID).ToList();

            }
            else
            {
                OrderDataGrid.ItemsSource = TradeEntities.GetContext().Order.Where(b => b.User.UserID == UserIDClass.UID &&
                    (b.OrderID.ToString().Contains(TextFilter) ||
                     b.OrderDate.ToString().Contains(TextFilter) ||
                     b.OrderDeliveryDate.ToString().Contains(TextFilter) ||
                     b.OrderPickupPoint1.OrderPickupPointAddress.Contains(TextFilter) ||
                     b.OrderCode.ToString().Contains(TextFilter))).ToList();
            }

            FilterCount.Text = OrderDataGrid.Items.Count + " из " + TradeEntities.GetContext().Order
                .Count(b => b.User.UserID == UserIDClass.UID);
        }

        public OrderUserPage()
        {
            InitializeComponent();
            var manufacturers = TradeEntities.GetContext().ProductManufacturer.ToList();
            FilterCount.Text =
                TradeEntities.GetContext().Order.Count(b => b.User.UserID == UserIDClass.UID) + " из " +
                TradeEntities.GetContext().Order.Count(b => b.User.UserID == UserIDClass.UID);
            OrderDataGrid.ItemsSource = TradeEntities.GetContext().Order.Where(b => b.User.UserID == UserIDClass.UID).ToList();
        }


        private void FilterProductTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterOrder(FilterAll.Text);
        }
        private void FilterProductSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            FilterOrder(FilterAll.Text);

        }

        private void GoBackClick(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void OrderDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var orderID = ((Order)OrderDataGrid.SelectedItem).OrderID;
                OrderProductDataGrid.ItemsSource =
                    TradeEntities.GetContext().OrderProduct.Where(b => b.OrderID == orderID).ToList();
            }
            catch (Exception)
            {
                //ignored
            }
        }
    }
}
