using Flowers.Model;
using Flowers.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Flowers.Views.Pages.AdminPages
{
    public partial class MainAdminPage : Page
    {
        private void FilterProduct(string TextFilter, string ManufFilter, string CostFilter)
        {
            ProductDataGrid.ItemsSource = null;
            if (CostFilter == "Сначала недорогие")
            {
                if (string.IsNullOrEmpty(TextFilter))
                {
                    if (ManufFilter == "Все производители")
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.ToList().OrderBy(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter).ToList().OrderBy(b => b.ProductCost);
                    }
                }
                else
                {
                    if (ManufFilter == "Все производители")
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductName1.ProductName1.Contains(TextFilter) ||
                            b.ProductDescription.Contains(TextFilter) ||
                            b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter)).ToList().OrderBy(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter &&
                            (b.ProductName1.ProductName1.Contains(TextFilter) ||
                            b.ProductDescription.Contains(TextFilter) ||
                            b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter))).ToList().OrderBy(b => b.ProductCost);
                    }
                }
            }
            if (CostFilter == "Сначала дорогие")
            {
                if (string.IsNullOrEmpty(TextFilter))
                {
                    if (ManufFilter == "Все производители")
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.ToList().OrderByDescending(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter).ToList().OrderByDescending(b => b.ProductCost);
                    }
                }
                else
                {
                    if (ManufFilter == "Все производители")
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductName1.ProductName1.Contains(TextFilter) ||
                            b.ProductDescription.Contains(TextFilter) ||
                            b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter)).ToList().OrderByDescending(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter &&
                            (b.ProductName1.ProductName1.Contains(TextFilter) ||
                            b.ProductDescription.Contains(TextFilter) ||
                            b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter))).ToList().OrderByDescending(b => b.ProductCost);
                    }
                }
            }
            FilterCount.Text = ProductDataGrid.Items.Count + " из " + TradeEntities.GetContext().Product.Count();
        }
        public MainAdminPage()
        {
            InitializeComponent();
            var manufacturers = TradeEntities.GetContext().ProductManufacturer.ToList();
            FilterManufactured.Items.Add("Все производители");
            FilterPrice.Items.Add("Сначала недорогие");
            FilterPrice.Items.Add("Сначала дорогие");
            foreach (var t in manufacturers)
            {
                FilterManufactured.Items.Add(t.ProductManufacturer1);
            }
            FilterProduct(FilterAll.Text, FilterManufactured.Text, FilterPrice.Text);
            FilterManufactured.SelectedIndex = 0;
            FilterPrice.SelectedIndex = 0;
            FilterCount.Text = TradeEntities.GetContext().Product.Count() + " из " + TradeEntities.GetContext().Product.Count();
            ProductDataGrid.ItemsSource = TradeEntities.GetContext().Product.ToList().OrderBy(n => n.ProductCost);
        }
        private void FilterProductTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterProduct(FilterAll.Text, FilterManufactured.Text, FilterPrice.Text);
        }
        private void FilterProductSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterManufactured.SelectedItem != null && FilterPrice.SelectedItem != null)
            {
                FilterProduct(FilterAll.Text, FilterManufactured.SelectedItem.ToString(), FilterPrice.SelectedItem.ToString());
            }
        }

        private void DeleteProductClick(object sender, RoutedEventArgs e)
        {
            var productID = ((Product)ProductDataGrid.SelectedItem).ProductArticleNumber;
            if (TradeEntities.GetContext().OrderProduct.Any(b => b.ProductID == productID && b.Order.OrderStatus == 2))
            {
                MessageBox.Show("Данный товар есть в заказе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MessageBox.Show("Вы действительно хотите удалить данный товар?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.No)
                return;
            try
            {
                var prodID = (Product)ProductDataGrid.SelectedItem;
                IEnumerable<OrderProduct> OrderProducts =
                    TradeEntities.GetContext().OrderProduct.Where(b => b.ProductID == prodID.ProductArticleNumber).ToList();
                TradeEntities.GetContext().OrderProduct.RemoveRange(OrderProducts);
                TradeEntities.GetContext().Product.Remove(prodID);
                TradeEntities.GetContext().SaveChanges();
                MessageBox.Show("Товар успешно удалён", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                FilterProduct(FilterAll.Text, FilterManufactured.Text, FilterPrice.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProductClick(object sender, RoutedEventArgs e)
        {
            var AEWindow = new AddEditWindow(null, false);
            AEWindow.ShowDialog();
            FilterProduct(FilterAll.Text, FilterManufactured.Text, FilterPrice.Text);
        }
        private void EditProductClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var AEWindow = new AddEditWindow(ProductDataGrid.SelectedItem as Product, true);
                AEWindow.ShowDialog();
                FilterProduct(FilterAll.Text, FilterManufactured.Text, FilterPrice.Text);
            }
            catch (Exception)
            {
                //ignored
            }
            ;
        }
    }
}
