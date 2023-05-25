using System;
using System.Collections.Generic;
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

namespace pirozhkov
.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageContent.xaml
    /// </summary>
    public partial class PageContent : Page
    {
        public PageContent()
        {
            InitializeComponent();
            if (App.CurrentUser != null)
            {
                switch (App.CurrentUser.Role.RoleName.ToLower())
                {
                    case "администратор":
                        BtnAdd.Visibility = Visibility.Visible;
                        break;
                    default:
                        BtnAdd.Visibility = Visibility.Collapsed;
                        break;
                }
                TbFullName.Content = App.CurrentUser.fullname;
            }
            else
            {
                BtnAdd.Visibility = Visibility.Collapsed;
            }
            

            ProductList.ItemsSource = App.Context.Product.ToList();

            var sortList = new List<string>()
            {
                "Без сортировки",
                "Цена по возрастанию",
                "Цена по убыванию"
                
            };
            CmbSort.ItemsSource = sortList;
            CmbSort.SelectedIndex = 0;

            var list = App.Context.Manufacturer.ToList();
            list.Insert(0, new Manufacturer { ManufacturerName = "Все производители" });
            CmbSearch.ItemsSource = list;
            CmbSearch.SelectedIndex = 0;

            int count = 0;
            foreach (var item in App.Context.Product.ToList())
            {
                count++;
            }

            ViewLabel.Content = count.ToString() + " из " + count.ToString();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateData();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }
        private void UpdateData()
        {

            var products = App.Context.Product.ToList();

            
            switch (CmbSort.SelectedIndex)
            {
                case 1:
                    products = products.OrderBy(p => p.ProductCost).ToList();
                    break;
                case 2:
                    products = products.OrderByDescending(p => p.ProductCost).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.ProductArticleNumber).ToList();
                    break;
            };
            if (CmbSearch.SelectedIndex != 0)
            {
                products = products.Where(p => p.Manufacturer == CmbSearch.SelectedItem as Manufacturer).ToList();
                ProductList.ItemsSource = products;
            }
            else
            {
                ProductList.ItemsSource = products;
            }
            products = products.Where(p => p.ProductName.ToLower().Contains(TbSearch.Text.ToLower()) ||
            p.ProductDescription.ToLower().Contains(TbSearch.Text.ToLower()) ||
            p.Manufacturer.ManufacturerName.ToLower().Contains(TbSearch.Text.ToLower()) ||
            p.ProductCost.ToString().Contains(TbSearch.Text.ToLower()) ||
            p.ProductQuantityInStock.ToString().Contains(TbSearch.Text.ToLower())).ToList();
            ProductList.ItemsSource = products;

            int _count = 0;
            foreach (var item in products)
            {
                _count++;
            }

            int count = 0;
            foreach (var item in App.Context.Product.ToList())
            {
                count++;
            }

            ViewLabel.Content = _count.ToString() + " из " + count.ToString();
        }

        private void CmbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Действительно удалить?", "Удалить", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                App.Context.Product.Remove((sender as Button).DataContext as Product);
                App.Context.SaveChanges();
                UpdateData();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Windows.AddEditProduct(null);
            if (window.ShowDialog() == true)
                UpdateData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Windows.AddEditProduct((sender as Button).DataContext as Product);
            if (window.ShowDialog() == true)
                UpdateData();
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ProductList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
