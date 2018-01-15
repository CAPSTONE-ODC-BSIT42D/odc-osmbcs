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
        public event EventHandler SaveCloseButtonClicked;

        protected virtual void OnAddingNewCustomer(RoutedEventArgs e)
        {
            var handler = AddNewCustomer;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.Customers.Where(x => x.CompanyName.ToLower().Contains(transSearchBoxSelectCustGridTb.Text.ToLower()));
            var observable = new ObservableCollection<Customer>(linqResults);
            selectCustomerDg.ItemsSource = observable;
        }

        private void selectCustBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
        private void addNewCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAddingNewCustomer(e);
        }
    }
}
