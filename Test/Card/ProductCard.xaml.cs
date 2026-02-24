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
    /// Логика взаимодействия для ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        public ProductCard(Product product)
        {
            InitializeComponent();
            DataContext = product;

            if (product.Discount > 0) 
            {
                TextPrice.TextDecorations = TextDecorations.Strikethrough;
            }
            else
            {
                TextPrice.Visibility = Visibility.Collapsed;
            }

            if (product.Discount > 15) 
            {
                var color = Color.FromRgb(46, 139, 87);
                DicountColor.Fill = new SolidColorBrush(color);
            }
            if (product.Count < 1) 
            {
                var conver = new BrushConverter();
                CountLable.Foreground = (Brush)conver.ConvertFromString("#FF0FFFD4");
            }
        }
    }
}
