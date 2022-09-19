using Flowers.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Flowers.Views.Pages.ManagerPages
{
    public partial class OrderManagerPage : Page
    {
        private void FilterOrder(string TextFilter)
        {
            OrderDataGrid.ItemsSource = null;

            if (string.IsNullOrEmpty(TextFilter))
            {

                OrderDataGrid.ItemsSource = TradeEntities.GetContext().Order.Where(b => b.OrderPickupPoint1.OrderPickupPointAddress == PPA && b.OrderDeliveryDate <= DateTime.Now && b.OrderStatus == 2).ToList();

            }
            else
            {
                OrderDataGrid.ItemsSource = TradeEntities.GetContext().Order.Where(b => b.OrderPickupPoint1.OrderPickupPointAddress == PPA && b.OrderDeliveryDate <= DateTime.Now && b.OrderStatus == 2 &&
                    (b.OrderID.ToString().Contains(TextFilter) ||
                    b.OrderDate.ToString().Contains(TextFilter) ||
                    b.OrderCode.ToString().Contains(TextFilter))).ToList();
            }

            FilterCount.Text = OrderDataGrid.Items.Count + " из " + TradeEntities.GetContext().Order
                .Count(b => b.OrderPickupPoint1.OrderPickupPointAddress == PPA && b.OrderDeliveryDate <= DateTime.Now && b.OrderStatus == 2);
        }

        private readonly string PPA;
        public OrderManagerPage(string ppa)
        {
            PPA = ppa;
            InitializeComponent();
            var manufacturers = TradeEntities.GetContext().ProductManufacturer.ToList();
            OrderDataGrid.SelectedIndex = 0;
            FilterCount.Text =
                TradeEntities.GetContext().Order.Count(b => b.OrderPickupPoint1.OrderPickupPointAddress == PPA && b.OrderStatus == 2 && b.OrderDeliveryDate <= DateTime.Now) + " из " +
                TradeEntities.GetContext().Order.Count(b => b.OrderPickupPoint1.OrderPickupPointAddress == PPA && b.OrderStatus == 2 && b.OrderDeliveryDate <= DateTime.Now);
            OrderDataGrid.ItemsSource = TradeEntities.GetContext().Order.Where(b => b.OrderPickupPoint1.OrderPickupPointAddress == PPA && b.OrderDeliveryDate <= DateTime.Now && b.OrderStatus == 2).ToList();
        }

        private void FilterProductTextChanged(object sender, TextChangedEventArgs e) => FilterOrder(FilterAll.Text);
        private void AllProductClick(object sender, System.Windows.RoutedEventArgs e) => NavigationService.Navigate(new ProductManagerPage());
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

        private void EndOrderClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите завершить заказ?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.No)
            {
                return;
            }

            try
            {
                var orderID = (Order)OrderDataGrid.SelectedItem;
                TradeEntities.GetContext().Order.First(b => b.OrderID == orderID.OrderID).OrderStatus = 1;
                TradeEntities.GetContext().SaveChanges();
                FilterOrder(FilterAll.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
