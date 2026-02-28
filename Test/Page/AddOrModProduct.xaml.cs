using Microsoft.EntityFrameworkCore;
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
using Test.Model;

namespace Test.Page
{
    /// <summary>
    /// Логика взаимодействия для AddOrModProduct.xaml
    /// </summary>
    public partial class AddOrModProduct : Window
    {
        private byte[] imageBytes = null;

        private Product products;

        public AddOrModProduct()
        {
            InitializeComponent();
            Title = "Создание";
            Load();

            AddButton.Content = "Создать";

            string l = "QWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            Random random = new Random();

            string id = "";

            for (int j = 0; j < 6; j++)
            {
                id += l[random.Next(0, l.Length)];
            }

            IdTextBox.Text = id;

            IdTextBox.IsEnabled = false;
        }

        public AddOrModProduct(Product product)
        {
            InitializeComponent();

            products = product;

            Title = "Редактирование";
            AddButton.Content = "Сохранить";

            DataContext = product;

            TitleTextBox.Text = product.Title;
            UnitTextBox.Text = product.Unit;
            PriceTextBox.Text = product.Price.ToString();
            DiscountTextBox.Text = product.Discount.ToString();
            CountTextBox.Text = product.Count.ToString();

            Load();

            TypeBootsComboBox.SelectedIndex = (int)product.TypeBootsId -1;
            ManufacturerComboBox.SelectedIndex = (int)product.ManufacturerId - 1;
            TypeProductComboBox.SelectedIndex = (int)product.TypeProductId - 1;
            SupplierComboBox.SelectedIndex = (int)product.SupplierId - 1;

            imageBytes = product.Photo;
            IdTextBox.Text = product.ProductId.ToString();
            IdTextBox.IsEnabled = false;

        }

        public void Load() 
        {
            var con = new DemoKrosContext();
            foreach (var item in con.TypeBoots.ToList()) 
            {
                TypeBootsComboBox.Items.Add(item.Title);
            }

            foreach (var item in con.Manufacturers.ToList())
            {
                ManufacturerComboBox.Items.Add(item.Title);
            }

            foreach (var item in con.TypeProducts.ToList())
            {
                TypeProductComboBox.Items.Add(item.Title);
            }

            foreach (var item in con.Suppliers.ToList())
            {
                SupplierComboBox.Items.Add(item.Title);
            }
        }

        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Изображение";
            if (op.ShowDialog() == true) 
            {
                imageBytes = File.ReadAllBytes(op.FileName);

                var a = new BitmapImage(new Uri(op.FileName));

                if (a.Width > 300 || a.Height > 200) 
                {
                    MessageBox.Show("Размер изображения не должен превышать 300x300 пикселей",
                       "Ошибка",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    return;
                }

                PhotoImage.Source = a;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            if (!int.TryParse(PriceTextBox.Text, out int price) || price < 1) 
            {
                MessageBox.Show("Цена должна быть числом и положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(CountTextBox.Text, out int count) || count < 1)
            {
                MessageBox.Show("Количество должна быть числом и положительным числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(DiscountTextBox.Text, out int disc) || disc < 0 || disc>100)
            {
                MessageBox.Show("Скидка должна быть числом и положительным числом и не превышать 99", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(TitleTextBox.Text) 
                || string.IsNullOrWhiteSpace(UnitTextBox.Text)
                || string.IsNullOrWhiteSpace(PriceTextBox.Text) 
                || string.IsNullOrWhiteSpace(DiscountTextBox.Text)
                || string.IsNullOrWhiteSpace(CountTextBox.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TitleTextBox.Text.Length > 500) 
            {
                MessageBox.Show("Сократите описание товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UnitTextBox.Text.Length > 100)
            {
                MessageBox.Show("Сократите текст единиц измерения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (products != null)
            {
                var con = new DemoKrosContext();
                Product product = con.Products.FirstOrDefault(i => i.ProductId == products.ProductId);

                product.Title = TitleTextBox.Text;
                product.TypeBootsId = TypeBootsComboBox.SelectedIndex + 1;
                product.ManufacturerId = ManufacturerComboBox.SelectedIndex + 1;
                product.TypeProductId = TypeProductComboBox.SelectedIndex + 1;
                product.SupplierId = SupplierComboBox.SelectedIndex + 1;

                product.Unit = UnitTextBox.Text;
                product.Price = Convert.ToInt32(PriceTextBox.Text);
                product.Discount = Convert.ToInt32(DiscountTextBox.Text);
                product.Count = Convert.ToInt32(CountTextBox.Text);

                product.Photo = imageBytes;

                con.SaveChanges();

            }
            else 
            {
                if (IdTextBox.Text.Length != 6) 
                {
                    MessageBox.Show("Код товара должен быть из 6 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var con = new DemoKrosContext();
                Product product = new Product();

                product.Title = TitleTextBox.Text;
                product.TypeBootsId = TypeBootsComboBox.SelectedIndex + 1;
                product.ManufacturerId = ManufacturerComboBox.SelectedIndex + 1;
                product.TypeProductId = TypeProductComboBox.SelectedIndex + 1;
                product.SupplierId = SupplierComboBox.SelectedIndex + 1;

                product.Unit = UnitTextBox.Text;
                product.Price = Convert.ToInt32(PriceTextBox.Text);
                product.Discount = Convert.ToInt32(DiscountTextBox.Text);
                product.Count = Convert.ToInt32(CountTextBox.Text);

                product.Photo = imageBytes;

                product.ProductId = IdTextBox.Text;

                con.Products.Add(product);

                con.SaveChanges();

                MessageBox.Show("Товар успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            new MainWindow().Show();
            Close();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
