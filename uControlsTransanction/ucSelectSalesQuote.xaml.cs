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

namespace prototype2.uControlsTransanction
{
    /// <summary>
    /// Interaction logic for ucSelectSalesQuote.xaml
    /// </summary>
    public partial class ucSelectSalesQuote : UserControl
    {
        public ucSelectSalesQuote()
        {
            InitializeComponent();
        }
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler SaveCloseOtherButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseOtherButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void findBtn_Click(object sender, RoutedEventArgs e)
        {
            var linqResults = MainVM.SalesQuotes.Where(x => x.sqNoChar_.ToLower().Contains(transSearchBoxSelectCustGridTb.Text.ToLower()) && !(x.status_.Equals("ACCEPTED")));
            var observable = new ObservableCollection<SalesQuote>(linqResults);
            selectSalesQuote.ItemsSource = observable;
        }

        private void selectSalesQuoteBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                if (MainVM.isNewPurchaseOrder)
                {
                    var observable = new ObservableCollection<SalesQuote>(from sq in MainVM.SalesQuotes where sq.status_.Equals("ACCEPTED") select sq);
                    selectSalesQuote.ItemsSource = observable;
                }
                else if (MainVM.isPaymentInvoice)
                {
                    var observable = new ObservableCollection<SalesQuote>(from sq in MainVM.SalesQuotes where sq.status_.Equals("PENDING") select sq);
                    selectSalesQuote.ItemsSource = observable;
                }
                
            }
        }
    }
}
