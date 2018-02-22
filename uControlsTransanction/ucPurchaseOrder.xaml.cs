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

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucPurchaseOrder.xaml
    /// </summary>
    public partial class ucPurchaseOrder : UserControl
    {
        public ucPurchaseOrder()
        {
            InitializeComponent();
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler SelectCustomer;
        protected virtual void OnSelectCustomerClicked(RoutedEventArgs e)
        {
            MainVM.isNewPurchaseOrder = true;
            var handler = SelectCustomer;
            if (handler != null)
                handler(this, e);
        }

        private void selectSupplierBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSelectCustomerClicked(e);
        }
    }
}
