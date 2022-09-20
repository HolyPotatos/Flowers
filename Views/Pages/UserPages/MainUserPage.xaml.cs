using Flowers.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Flowers.Views.Pages.UserPages
{
    public partial class MainUserPage : Page
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
        public MainUserPage()
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
            CartCountTBox.Text = "0";
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

        private List<Product> CartProducts = new List<Product>();
        private void CartAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (CartProducts.Count(b => b.ProductArticleNumber == (ProductDataGrid.SelectedItem as Product).ProductArticleNumber) > (ProductDataGrid.SelectedItem as Product).ProductQuantityInStock - 1)
            {
                MessageBox.Show("Товар закончился", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            CartProducts.Add(ProductDataGrid.SelectedItem as Product);
            CartCountTBox.Text = CartProducts.Count.ToString();
        }
        private void NavigateOrderUserPageClick(object sender, RoutedEventArgs e) => NavigationService.Navigate(new OrderUserPage());
        private void NavigateCartUserPageClick(object sender, RoutedEventArgs e) => NavigationService.Navigate(new CartUserPage(CartProducts));
    }
}
