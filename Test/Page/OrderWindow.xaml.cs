using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using Test.Card;
using Test.Model;

namespace Test.Page
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        Person Person = new Person();

        public OrderWindow(Person person)
        {
            InitializeComponent();
            Person = person;

            if (person.TypeRoleId == 3) 
            {
                OrderList.ContextMenu = new ContextMenu();
                AddButton.Visibility = Visibility.Collapsed;
            }

            Load();
        }

        public void Load() 
        {
            OrderList.Items.Clear();
            var con = new DemoKrosContext();
            foreach (var item in con.Orders.Include(i=>i.DeliviryPoint)) 
            {
                OrderList.Items.Add(new OrderCard(item));
            }
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(Person).Show();
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddOrModOrder(Person).Show();
            Close();
        }

        private void ModButton_Click(object sender, RoutedEventArgs e)
        {
            var i = OrderList.SelectedItem as OrderCard;
            var it = i.DataContext as Order;

            if (it.Status == "Завершён") 
            {
                MessageBox.Show("Заказ завершен","Информация",MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }

            new AddOrModOrder(Person, it).Show();
            Close();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var mess = MessageBox.Show("Точно удалять?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mess == MessageBoxResult.Yes)
            {
                var con = new DemoKrosContext();
                var i = OrderList.SelectedItem as OrderCard;
                var it = i.DataContext as Order;

                if (it.Status == "Завершён")
                {
                    MessageBox.Show("Заказ завершен невозможно удалить", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                var li = con.OredersItems.Where(i=>i.OrdersId == it.OrdersId);
                foreach (var item in li) 
                {
                    con.OredersItems.Remove(item);
                }

                con.Orders.Remove(it);

                con.SaveChanges();

                Load();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(Person).Show();
            Close();
        }
    }
}
