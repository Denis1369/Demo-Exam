using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Test.Card;
using Test.Model;
using Test.Page;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Person User = new Person();

        public MainWindow()
        {
            InitializeComponent();
            Load();
            ProductList.ContextMenu = null;
            OredrButton.Visibility = Visibility.Collapsed;
            AddButton.Visibility = Visibility.Collapsed;
            FindStack.Visibility = Visibility.Collapsed;
            FilStack.Visibility = Visibility.Collapsed;
            SortStack.Visibility = Visibility.Collapsed;
        }

        public MainWindow(Person user)
        {
            InitializeComponent();
            Load();
            NameLable.Content = user.LastName + " " + user.Name;
            if (user.TypeRole.Title == "Менеджер")
            {
                ProductList.ContextMenu = null;
                AddButton.Visibility = Visibility.Collapsed;
            }
            if (user.TypeRole.Title == "Авторизированный клиент")
            {
                ProductList.ContextMenu = null;
                OredrButton.Visibility = Visibility.Collapsed;
                AddButton.Visibility = Visibility.Collapsed;
                FindStack.Visibility = Visibility.Collapsed;
                FilStack.Visibility = Visibility.Collapsed;
                SortStack.Visibility = Visibility.Collapsed;
            }
            User = user;
        }

        public void Load() 
        {
            var con = new DemoKrosContext();
            var productList = con.Products.Include(p => p.Manufacturer)
                .Include(p => p.Supplier)
                .Include(p => p.TypeBoots)
                .Include(p => p.TypeProduct);

            FilterComboBox.Items.Add("Все");

            foreach (var item in con.Suppliers.ToList()) 
            {
                FilterComboBox.Items.Add(item.Title);
            }

            foreach (var item in productList) 
            {
                ProductList.Items.Add(new ProductCard(item));
            }

            SortComboBox.Items.Add("По возрастанию");
            SortComboBox.Items.Add("По убыванию");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            Close();
        }

        private void ModMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ProductCard)ProductList.SelectedItem;
            var item = selectedItem.DataContext as Product;

            new AddOrModProduct(item).Show();
            Close();
        }

        private void DelMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedCard = ProductList.SelectedItem as ProductCard;
            if (selectedCard == null) return;

            var product = selectedCard.DataContext as Product;
            if (product == null) return;

            try
            {
                var r = MessageBox.Show("Точно удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (r == MessageBoxResult.Yes) 
                {
                    using var con = new DemoKrosContext();

                    var dbProduct = con.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                    if (dbProduct == null) return;

                    con.Products.Remove(dbProduct);
                    con.SaveChanges();

                    Find();
                }
            }
            catch
            {
                MessageBox.Show("Невозможно удалить: товар используется");
            }
        }


        public void Find() 
        {
            ProductList.Items.Clear();

            string filtr = FilterComboBox.SelectedItem.ToString();

            var con = new DemoKrosContext();

            IEnumerable<Product> list = con.Products.Include(p => p.Manufacturer)
                    .Include(p => p.Supplier)
                    .Include(p => p.TypeBoots)
                    .Include(p => p.TypeProduct).AsQueryable();

            if (filtr != "Все") {
                list = list.Where(p => p.Supplier.Title == filtr);
            }

            if (!string.IsNullOrWhiteSpace(SerchTextBox.Text))
            {
                string search = SerchTextBox.Text.ToLower();
                list = list.Where(item => item.TypeBoots.Title.ToLower().Contains(search) ||
                                            item.Title.ToLower().Contains(search));
            }

            switch (SortComboBox.SelectedValue)
            {
                case "По возрастанию":
                    list = list.OrderBy(i => i.Count);
                    break;
                case "По убыванию":
                    list = list.OrderByDescending(i => i.Count);
                    break;
                }

            foreach (var item in list) 
            {
                ProductList.Items.Add(new ProductCard(item));
            }
        }

        private void FilterComboBox_DropDownClosed(object sender, EventArgs e)
        {
            Find();
        }

        private void SerchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Find();
        }

        private void SortComboBox_DropDownClosed(object sender, EventArgs e)
        {
            Find();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddOrModProduct().Show();
            Close();
        }

        private void OredrButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow(User).Show();
            Close();
        }
    }
}