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
using Test.Model;

namespace Test.Card
{
    /// <summary>
    /// Логика взаимодействия для OrderCard.xaml
    /// </summary>
    public partial class OrderCard : UserControl
    {
        Order Order = new Order();
        public OrderCard(Order order )
        {
            InitializeComponent();
            DataContext = order;
            Order = order;
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var con = new DemoKrosContext();
            var list = con.OredersItems.Where(i=> i.OrdersId == Order.OrdersId);
            string mess = "";
            foreach (var item in list) 
            {
                mess += "Артикул: " + item.ProductId + "    Количество: " + item.Count;
                mess += "\n";
            }
            MessageBox.Show(mess, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
