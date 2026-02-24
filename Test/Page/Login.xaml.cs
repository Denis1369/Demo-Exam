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
using Test.Model;

namespace Test.Page
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButtton_Click(object sender, RoutedEventArgs e)
        {
            var con = new DemoKrosContext();
            Person user = con.People.FirstOrDefault(p => p.Login == LoginText.Text && p.Pass == PassText.Text);

            if (user != null)
            {
                var us = con.People.Include(p => p.TypeRole).FirstOrDefault(u => u.PersonId == user.PersonId);
                new MainWindow(us).Show();
                Close();
            }

            else 
            {
                MessageBox.Show("Невеные данные", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GeustButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();    
            Close();
        }
    }
}
