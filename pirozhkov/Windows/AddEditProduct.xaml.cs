using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace pirozhkov
.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditProduct.xaml
    /// </summary>
    public partial class AddEditProduct : Window
    {
        Product _product;
        private byte[] _photo = null;
        bool _save = false;

        public AddEditProduct(Product product)
        {
            InitializeComponent();
            var categorylist = App.Context.ProductCategory.ToList();
            CmbCategory.ItemsSource = categorylist;

            var manyflist = App.Context.Manufacturer.ToList();
            CmbManufacturer.ItemsSource = manyflist;

            var provlist = App.Context.Provider.ToList();
            CmbProvider.ItemsSource = provlist;

            if (product == null)
            {
                _save = true;
                _product = new Product();
            }
            else
            {
                _product = product;
                _photo = _product.ProductPhoto;
            }
            DataContext = _product;
        }

        private void BtnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Image files | *.png; *jpg; *.jpeg"
            };
            if (ofd.ShowDialog().Value)
            {
                _photo = File.ReadAllBytes(ofd.FileName);
                ImagePhoto.Source = new BitmapImage(new Uri(ofd.FileName));
                ImagePhoto.DataContext = _photo;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            decimal num;
            var products = this.DataContext as Product;
            string error = "";
            try
            {
                if (string.IsNullOrEmpty(products.ProductArticleNumber))
                    error += "Вы не ввели артикул\n";
                if (string.IsNullOrEmpty(products.ProductName))
                    error += "Вы не ввели наименование\n";
                if (string.IsNullOrEmpty(products.ProductDescription))
                    error += "Вы не ввели описание\n";
                if (products.ProductCategory1 == null)
                    error += "Вы не выбрали категорию\n";
                if (products.Manufacturer == null)
                    error += "Вы не выбрали производителя\n";
                if (products.Provider == null)
                    error += "Вы не выбрали поставщика\n";
                //if (!decimal.TryParse(TbCost.Text, out num))
                //    error += "Неверный формат цены\n";
                if (products.ProductCost <= 0 || string.IsNullOrWhiteSpace(products.ProductCost.ToString()))
                    error += "Неверная цена\n";
                if (!decimal.TryParse(TbTbMaxDiscount.Text, out num))
                    error += "Неверный формат макс. скидки\n";
                if (products.ProductMaxDiscount < 0 || products.ProductMaxDiscount > 100 || string.IsNullOrWhiteSpace(products.ProductMaxDiscount.ToString()))
                    error += "Неверная макс. скидка\n";
                if (!decimal.TryParse(TbDiscount.Text, out num))
                    error += "Неверный формат скидки\n";
                if (products.ProductDiscountAmount <= 0 || string.IsNullOrWhiteSpace(products.ProductDiscountAmount.ToString()))
                    error += "Неверная скидка\n";
                if (products.ProductDiscountAmount > products.ProductMaxDiscount)
                    error += "Слишком большая скидка\n";
                if (!decimal.TryParse(TbQuant.Text, out num))
                    error += "Неверный формат кол-ва\n";
                if (products.ProductQuantityInStock < 0 || string.IsNullOrWhiteSpace(products.ProductQuantityInStock.ToString()))
                    error += "Неверное кол-во\n";
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show(error);
                    error = "";

                }
                else
                {
                    if (_save != true)
                    {
                        _product.ProductPhoto = _photo;
                        App.Context.SaveChanges();
                        DialogResult = true;
                    }
                    else
                    {
                        _product.ProductPhoto = _photo;
                        App.Context.Product.Add(_product);
                        App.Context.SaveChanges();
                        DialogResult = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка в сохранении");
                
            }
            
        }
    }
}
