using Flowers.Model;
using Flowers.Model.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Flowers.Views.Pages.UserPages
{
    public partial class CartUserPage : Page
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
                        ProductDataGrid.ItemsSource = CartProducts.OrderBy(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = CartProducts.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter).ToList().OrderBy(b => b.ProductCost);
                    }
                }
                else
                {
                    if (ManufFilter == "Все производители")
                    {
                        ProductDataGrid.ItemsSource = CartProducts.Where(b => b.ProductName1.ProductName1.Contains(TextFilter) ||
                                                                              b.ProductDescription.Contains(TextFilter) ||
                                                                              b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter)).ToList().OrderBy(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = CartProducts.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter &&
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
                        ProductDataGrid.ItemsSource = CartProducts.ToList().OrderByDescending(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = CartProducts.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter).ToList().OrderByDescending(b => b.ProductCost);
                    }
                }
                else
                {
                    if (ManufFilter == "Все производители")
                    {
                        ProductDataGrid.ItemsSource = CartProducts.Where(b => b.ProductName1.ProductName1.Contains(TextFilter) ||
                                                                              b.ProductDescription.Contains(TextFilter) ||
                                                                              b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter)).ToList().OrderByDescending(b => b.ProductCost);
                    }
                    else
                    {
                        ProductDataGrid.ItemsSource = CartProducts.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter &&
                                                                              (b.ProductName1.ProductName1.Contains(TextFilter) ||
                                                                               b.ProductDescription.Contains(TextFilter) ||
                                                                               b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter))).ToList().OrderByDescending(b => b.ProductCost);
                    }
                }
            }
            FilterCount.Text = ProductDataGrid.Items.Count + " из " + CartProducts.Count();
        }

        readonly List<Product> CartProducts;

        private void SetActualPrice() => PriceTBlock.Text = "Итоговая цена " + ((int)ProductDataGrid.Items.Cast<object>().Sum(b => ((Product)b).ProductCost)).ToString() + " руб.";
        
        public CartUserPage(List<Product> CP)
        {
            InitializeComponent();
            CartProducts = CP;
            PickupPointCBox.ItemsSource = TradeEntities.GetContext().OrderPickupPoint.ToList();
            PickupPointCBox.SelectedIndex = 0;
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
            FilterCount.Text = CartProducts.Count + " из " + CartProducts.Count;
            ProductDataGrid.ItemsSource = CartProducts;
            SetActualPrice();
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

        private void CartBuyButtonClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы подтверждаете платёж?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
            {
                // имитация оплаты
                try
                {
                    int CodeOrder;
                    var Rand = new Random();
                    do
                    {
                        CodeOrder = Rand.Next(100000, 999999);
                    } while (TradeEntities.GetContext().Order.Any(b => b.OrderCode == CodeOrder));

                    var opp = (PickupPointCBox.SelectedItem as OrderPickupPoint);
                    var order = new Order
                    {
                        OrderStatus = 2,
                        OrderDate = DateTime.Now,
                        OrderDeliveryDate = DateTime.Now + TimeSpan.FromDays(5),
                        OrderPickupPoint = TradeEntities.GetContext().OrderPickupPoint.First(b =>
                            b.OrderPickupPointAddress == opp.OrderPickupPointAddress).OrderPickupPointID,
                        OrderCode = CodeOrder,
                        ClientData = TradeEntities.GetContext().User.First(b => b.UserID == UserIDClass.UID).UserID
                    };
                    TradeEntities.GetContext().Order.Add(order);
                    TradeEntities.GetContext().SaveChanges();
                    while (true)
                    {
                        var Article = CartProducts[0].ProductArticleNumber;
                        var Count = CartProducts.Count(b => b.ProductArticleNumber == Article);
                        var orderProduct = new OrderProduct
                        {
                            OrderID = order.OrderID,
                            ProductCount = Count,
                            ProductID = Article,
                        };
                        TradeEntities.GetContext().OrderProduct.Add(orderProduct);
                        TradeEntities.GetContext().Product.First(b => b.ProductArticleNumber == Article).ProductQuantityInStock -= Count;
                        TradeEntities.GetContext().SaveChanges();
                        CartProducts.RemoveAll(b => b.ProductArticleNumber == Article);
                        if (CartProducts.Count == 0)
                        {
                            break;
                        }
                    }
                    ProductDataGrid.ItemsSource = null;
                    SetActualPrice();
                    MessageBox.Show("Заказ успешно оформлен!\nТовар придёт через 5 дней.","Уведомление",MessageBoxButton.OK,MessageBoxImage.Asterisk);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GoBackButtonClick(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
