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

namespace prototype2
{
    /// <summary>
    /// Interaction logic for addNewItem.xaml
    /// </summary>
    public partial class addNewItem : Window
    {
        public addNewItem()
        {
            InitializeComponent();
            productRbtn.IsChecked = true;
            this.DataContext = MainMenu.MainVM;
        }

        private void productRbtn_Checked(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < forms.Children.Count; x++)
            {
                forms.Children[x].Visibility = Visibility.Hidden;
            }
            product.Visibility = Visibility.Visible;
            service.Visibility = Visibility.Hidden;
        }

        private void productRbtn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void serviceRbtn_Checked(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < forms.Children.Count; x++)
            {
                forms.Children[x].Visibility = Visibility.Hidden;
            }
            product.Visibility = Visibility.Hidden;
            service.Visibility = Visibility.Visible;
        }

        private void serviceRbtn_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach(Item prd in MainMenu.MainVM.ProductList)
            {
                if (prd.IsChecked)
                {
                    MainMenu.MainVM.RequestedItems.Add(new RequestedItem() { lineNo = (MainMenu.MainVM.RequestedItems.Count + 1).ToString(), itemCode = prd.ItemNo, desc = prd.ItemDesc, qty = prd.Quantity});
                }
            }
        }
    }
}
