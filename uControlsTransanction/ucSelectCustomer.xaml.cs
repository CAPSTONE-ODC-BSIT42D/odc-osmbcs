using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace prototype2.uControlsMaintenance
{
    /// <summary>
    /// Interaction logic for ucSelectCustomer.xaml
    /// </summary>
    public partial class ucSelectCustomer : UserControl
    {
        public ucSelectCustomer()
        {
            InitializeComponent();
        }
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler AddNewCustomer;

        protected virtual void OnAddingNewCustomer(RoutedEventArgs e)
        {
            var handler = AddNewCustomer;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler SaveCloseOtherButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseOtherButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.isNewPurchaseOrder && this.IsVisible)
            {
                var observable = new ObservableCollection<Customer>(from supp in MainVM.Suppliers where supp.CompanyName.Contains(transSearchBoxSelectCustGridTb.Text) && supp.CompanyType == 1 select supp);
                selectCustomerDg.ItemsSource = observable;
            }
            else
            {
                var observable = new ObservableCollection<Customer>(from cust in MainVM.Customers where cust.CompanyName.Contains(transSearchBoxSelectCustGridTb.Text) select cust);
                selectCustomerDg.ItemsSource = observable;
            }
            
        }

        private void selectCustBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
        private void addNewCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAddingNewCustomer(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainVM.isNewPurchaseOrder && this.IsVisible)
            {
                selectCustomerDg.Visibility = Visibility.Collapsed;
                selectSupplierDg.Visibility = Visibility.Visible;
                headerLbl.Content = "Select a Supplier";
            }

        }
    }
}
