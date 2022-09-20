using Flowers.Model;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Flowers.Views.Windows
{
    public partial class AddEditWindow : Window
    {
        private Product product = new Product();
        private bool IsEdit;
        public AddEditWindow(Product pr, bool isEdit)
        {
            InitializeComponent();
            IsEdit = isEdit;
            if (pr != null)
                product = pr;
            Title = isEdit ? "Редактирование" : "Создание";
            DataContext = product;
            CategoryCBox.ItemsSource = TradeEntities.GetContext().ProductCategory.ToList();
            NameCBox.ItemsSource = TradeEntities.GetContext().ProductName.ToList();
            ManufCBox.ItemsSource = TradeEntities.GetContext().ProductManufacturer.ToList();
            MeasureCBox.ItemsSource = TradeEntities.GetContext().ProductMeasure.ToList();
            SupplierCBox.ItemsSource = TradeEntities.GetContext().ProductSupplier.ToList();
            if (isEdit)
            {
                CategoryCBox.SelectedItem = product.ProductCategory1;
                NameCBox.SelectedItem = product.ProductName1;
                ManufCBox.SelectedItem = product.ProductManufacturer1;
                MeasureCBox.SelectedItem = product.ProductMeasure1;
                SupplierCBox.SelectedItem = product.ProductSupplier1;
                ArticleTBox.IsReadOnly = true;
            }
        }
        private void CloseClick(object sender, RoutedEventArgs e) => Close();
        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "1234567890.".IndexOf(e.Text) < 0;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(ArticleTBox.Text))
                errors.AppendLine("Введите артикул");
            if (string.IsNullOrEmpty(DescriptionTBox.Text))
                errors.AppendLine("Введите описание");
            if (string.IsNullOrEmpty(CountTBox.Text))
                errors.AppendLine("Введите количество");
            if (string.IsNullOrEmpty(PriceTBox.Text))
                errors.AppendLine("Введите цену");
            if (string.IsNullOrEmpty(MaxDiscountTBox.Text))
                errors.AppendLine("Введите максимально возможную скидку");
            if (string.IsNullOrEmpty(CurrentDiscountTBox.Text))
                errors.AppendLine("Введите текущую скидку");
            if (NameCBox.SelectedIndex == -1)
                errors.AppendLine("Выберите наименование");
            if (ManufCBox.SelectedIndex == -1)
                errors.AppendLine("Выберите производителя");
            if (SupplierCBox.SelectedIndex == -1)
                errors.AppendLine("Выберите поставщика");
            if (CategoryCBox.SelectedIndex == -1)
                errors.AppendLine("Выберите категорию");
            if (MeasureCBox.SelectedIndex == -1)
                errors.AppendLine("Выберите единицы измерения");
            if (TradeEntities.GetContext().Product.Any(b => b.ProductArticleNumber == ArticleTBox.Text) && IsEdit == false)
                errors.AppendLine("Товар с таким артикулом уже есть");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (IsEdit)
                {
                    product.ProductName = ((ProductName)NameCBox.SelectedItem).ProductNameID;
                    product.ProductSupplier = ((ProductSupplier)SupplierCBox.SelectedItem).ProductSupplierID;
                    product.ProductManufacturer = ((ProductManufacturer)ManufCBox.SelectedItem).ProductManufacturerID;
                    product.ProductCategory = ((ProductCategory)CategoryCBox.SelectedItem).ProductCategoryID;
                    product.ProductMeasure = ((ProductMeasure)MeasureCBox.SelectedItem).ProductMeasureID;
                    TradeEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация о товаре успешно обновлена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Close();
                }
                else
                {
                    var product = new Product();
                    product.ProductArticleNumber = ArticleTBox.Text;
                    product.ProductName = ((ProductName)NameCBox.SelectedItem).ProductNameID;
                    product.ProductSupplier = ((ProductSupplier)SupplierCBox.SelectedItem).ProductSupplierID;
                    product.ProductManufacturer = ((ProductManufacturer)ManufCBox.SelectedItem).ProductManufacturerID;
                    product.ProductCategory = ((ProductCategory)CategoryCBox.SelectedItem).ProductCategoryID;
                    product.ProductMeasure = ((ProductMeasure)MeasureCBox.SelectedItem).ProductMeasureID;
                    product.ProductCost = decimal.Parse(PriceTBox.Text);
                    product.ProductDiscountAmount = byte.Parse(CurrentDiscountTBox.Text);
                    product.ProductDiscountPossible = byte.Parse(MaxDiscountTBox.Text);
                    product.ProductQuantityInStock = int.Parse(CountTBox.Text);
                    product.ProductDescription = DescriptionTBox.Text;
                    product.ProductPhoto = ImageData;
                    TradeEntities.GetContext().Product.Add(product);
                    TradeEntities.GetContext().SaveChanges();
                    MessageBox.Show("Товар успешно добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }


        }

        private void MaxDiscountController(object sender, TextChangedEventArgs e)
        {
            if (byte.Parse(CurrentDiscountTBox.Text) > byte.Parse(MaxDiscountTBox.Text))
            {
                CurrentDiscountTBox.Text = MaxDiscountTBox.Text;
            }
        }

        private void DeletePhotoClick(object sender, RoutedEventArgs e)
        {
            product.ProductPhoto = null;
            PhotoImage.Source = null;
            MessageBox.Show("Фотография товара удалена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        byte[] ImageData = null;
        private void SetProductPhotoClick(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Файлы (*.png)|*.png|JPEG Файлы (*.jpg)|*.jpg|BMP Файлы (*.bmp)|*.bmp";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                var filename = dlg.FileName;

                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    ImageData = new byte[fs.Length];
                    fs.Read(ImageData, 0, ImageData.Length);
                    var ms = new MemoryStream(ImageData);
                    var currentImage = new BitmapImage();
                    currentImage.BeginInit();
                    currentImage.StreamSource = ms;
                    currentImage.EndInit();
                    PhotoImage.Source = currentImage;
                }
                product.ProductPhoto = ImageData;
            }
        }
    }
}
