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
using Test.Model;

namespace Test.Page
{
    /// <summary>
    /// Логика взаимодействия для AddOrModOrder.xaml
    /// </summary>
    public partial class AddOrModOrder : Window
    {
        public List<OredersItem> oredersItems = new List<OredersItem>();

        public Order Order = new Order();
        Person Person = new Person();


        public AddOrModOrder(Person person)
        {
            InitializeComponent();

            Person = person;

            var con = new DemoKrosContext();

            AddButton.Content = "Создать";

            try
            {
                Order.OrdersId = con.Orders.OrderBy(i=>i.OrdersId).Last().OrdersId + 1;
            }
            catch (Exception ex) 
            {
                Order.OrdersId = 1;
            }

            StausCombobox.SelectedIndex = 0;

            StausCombobox.IsEnabled = false;

            Load();
        }

        public AddOrModOrder(Person person, Order order)
        {
            InitializeComponent();
            Person = person;

            Order = order;
            Load();
            AddButton.Content = "Редактировать";
            DateOreder.SelectedDate = order.DateDeliviry?.ToDateTime(TimeOnly.MinValue);

            var con = new DemoKrosContext();

            var li = con.OredersItems.Where(i => i.OrdersId == order.OrdersId).ToList();
            oredersItems = li;
            LoadItem();

        }

        public void Load()
        {
            DateOreder.DisplayDateStart = DateTime.Now;

            var con = new DemoKrosContext();

            StausCombobox.Items.Add("Новый");
            StausCombobox.Items.Add("Завершить");

            foreach (var item in con.DeliviryPoints.ToList()) 
            {
                DelirivyCombobox.Items.Add(item.TitleAll);
                if (Order.DeliviryPointId == item.DeliviryPointId)
                {
                    DelirivyCombobox.SelectedIndex = DelirivyCombobox.Items.Count-1;
                }
            }

            foreach (var item in con.Products.ToList()) 
            {
                ProductCombobox.Items.Add(item.ProductId);
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow(Person).Show();
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var con = new DemoKrosContext();

            if (DateOreder.SelectedDate == null)
            {
                MessageBox.Show("Выберете дату доставки");
                return;
            }

            if (DelirivyCombobox.SelectedItem == null)
            {
                MessageBox.Show("Выберете точку доставки");
                return;
            }

            string sel = DelirivyCombobox.SelectedItem.ToString();

            var delpoint = con.DeliviryPoints
                .FirstOrDefault(d => (d.City + " " + d.Street + " " + d.Number) == sel);

            if (delpoint == null)
            {
                MessageBox.Show("Выберете точку доставки");
                return;
            }

            Order.DeliviryPointId = delpoint.DeliviryPointId;
            Order.DateDeliviry = DateOnly.FromDateTime((DateTime)DateOreder.SelectedDate);
            Order.Status = StausCombobox.SelectedItem?.ToString();
            Order.PersonId = Person.PersonId;

            if (!con.Orders.Any(o => o.OrdersId == Order.OrdersId))
            {
                Order.DateOrder = DateOnly.FromDateTime(DateTime.Now);

                con.Orders.Add(Order);

                foreach (var item in oredersItems)
                {
                    con.OredersItems.Add(new OredersItem()
                    {
                        OrdersId = Order.OrdersId,
                        ProductId = item.ProductId,
                        Count = item.Count
                    });
                }

                con.SaveChanges();
                MessageBox.Show("Заказ создан");
            }
            else
            {
                var dbOrder = con.Orders.FirstOrDefault(o => o.OrdersId == Order.OrdersId);
                if (dbOrder == null)
                {
                    MessageBox.Show("Заказ не найден в базе");
                    return;
                }

                dbOrder.DeliviryPointId = Order.DeliviryPointId;
                dbOrder.DateDeliviry = Order.DateDeliviry;
                dbOrder.Status = Order.Status;

                var oldItems = con.OredersItems.Where(i => i.OrdersId == dbOrder.OrdersId).ToList();
                con.OredersItems.RemoveRange(oldItems);

                foreach (var item in oredersItems)
                {
                    con.OredersItems.Add(new OredersItem()
                    {
                        OrdersId = dbOrder.OrdersId,
                        ProductId = item.ProductId,
                        Count = item.Count
                    });
                }

                con.SaveChanges();
                MessageBox.Show("Заказ обновлён");
            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (ProductCombobox.SelectedItem.ToString() == null) 
            {
                MessageBox.Show("Выберете товар");
                return;
            }

            var con = new DemoKrosContext();

            Product product = con.Products.FirstOrDefault(i=>i.ProductId== ProductCombobox.SelectedItem.ToString());

            if (!int.TryParse(CountText.Text, out int count) || count < 1) 
            {
                MessageBox.Show("Количество должно быть положительным числом");
                return;
            }

            var existingItem = oredersItems.FirstOrDefault(i => i.ProductId == product.ProductId);


            if (existingItem != null)
            {
                var i = con.Products.FirstOrDefault(it=> it.ProductId == existingItem.ProductId);

                int count_sum = count + (int)existingItem.Count;
                
                if (count_sum > i.Count) 
                {
                    MessageBox.Show("Товара меньше на складе","Уведомление"
                        ,MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }
                existingItem.Count = count_sum;
            }
            else
            {
                OredersItem item = new OredersItem()
                {
                    ProductId = product.ProductId,
                    Count = count,
                };

                var i = con.Products.FirstOrDefault(it => it.ProductId == item.ProductId);

                if (item.Count > i.Count)
                {
                    MessageBox.Show("Товара меньше на складе", "Уведомление"
                        , MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                oredersItems.Add(item);
            }
            LoadItem();
        }

        public void LoadItem() 
        {
            ItemList.Items.Clear();
            foreach (var item in oredersItems) 
            {
                ItemList.Items.Add($"{item.ProductId} | {item.Count}");
            }
        }

        private void DelItem_Click(object sender, RoutedEventArgs e)
        {
            if (ItemList.SelectedValue == null) 
            {
                MessageBox.Show("Выберете товар для удаления");
                return;
            }
            oredersItems.RemoveAt(ItemList.SelectedIndex);

            LoadItem();
        }

        private void StausCombobox_DropDownClosed(object sender, EventArgs e)
        {
            var con = new DemoKrosContext();

            if (StausCombobox.SelectedValue.ToString() == "Завершить") 
            {
                if (DateOnly.FromDateTime(DateTime.Now) < Order.DateDeliviry) 
                {
                    MessageBox.Show("Нельзя забратьт раньше для доставки");
                    return;
                }


                foreach (var item in oredersItems) 
                {
                    var it = con.Products.FirstOrDefault(i=>i.ProductId == item.ProductId);
                    it.Count -= item.Count;
                    con.SaveChanges();
                }

                Order.Status = "Завершён";

                var dbOrder = con.Orders.FirstOrDefault(o => o.OrdersId == Order.OrdersId);
                dbOrder.Status = "Завершён";
                dbOrder.DeliviryPointId = Order.DeliviryPointId;
                dbOrder.DateDeliviry = Order.DateDeliviry;

                con.SaveChanges();
                MessageBox.Show("Забрали заказ");
                new OrderWindow(Person).Show();
                Close();


            }
        }
    }
}
