﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Flowers.Model;

namespace Flowers.Views.Pages.GuestPages
{
    public partial class MainGuestPage : Page
    {
        private void FilterProduct(string TextFilter, string ManufFilter, string CostFilter)
        {
            dgrid.ItemsSource = null;
            if (CostFilter == "Сначала недорогие")
            {
                if (string.IsNullOrEmpty(TextFilter))
                {
                    if (ManufFilter == "Все производители")
                    {
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.ToList().OrderBy(b => b.ProductCost);
                    }
                    else
                    {
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter).ToList().OrderBy(b => b.ProductCost);
                    }
                }
                else
                {
                    if (ManufFilter == "Все производители")
                    {
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductName1.ProductName1.Contains(TextFilter) ||
                            b.ProductDescription.Contains(TextFilter) ||
                            b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter)).ToList().OrderBy(b => b.ProductCost);
                    }
                    else
                    {
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter && 
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
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.ToList().OrderByDescending(b => b.ProductCost);
                    }
                    else
                    {
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter).ToList().OrderByDescending(b => b.ProductCost);
                    }
                }
                else
                {
                    if (ManufFilter == "Все производители")
                    {
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductName1.ProductName1.Contains(TextFilter) ||
                            b.ProductDescription.Contains(TextFilter) ||
                            b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter)).ToList().OrderByDescending(b => b.ProductCost);
                    }
                    else
                    {
                        dgrid.ItemsSource = TradeEntities.GetContext().Product.Where(b => b.ProductManufacturer1.ProductManufacturer1 == ManufFilter &&
                            (b.ProductName1.ProductName1.Contains(TextFilter) ||
                            b.ProductDescription.Contains(TextFilter) ||
                            b.ProductManufacturer1.ProductManufacturer1.Contains(TextFilter))).ToList().OrderByDescending(b => b.ProductCost);
                    }
                }
            }
            FilterCount.Text = dgrid.Items.Count + " из " + TradeEntities.GetContext().Product.Count();
        }
        public MainGuestPage()
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
            dgrid.ItemsSource = TradeEntities.GetContext().Product.ToList().OrderBy(n => n.ProductCost);
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

    }
}
