using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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
    /// Interaction logic for ucInvoice.xaml
    /// </summary>
    public partial class ucInvoice : UserControl
    {
        public ucInvoice()
        {
            InitializeComponent();
        }

        private bool validationError = false;
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        DateTime dateOfIssue = new DateTime();
        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }
        Document document;

        private void UserControlInvoice_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (this.IsVisible)
            {
                changeGridView();
            }
            
        }


        void changeGridView()
        {
            foreach (Grid gr in InvoiceGrid.Children)
            {
                gr.Visibility = Visibility.Collapsed;
            }
            if (MainVM.isViewHome)
            {
                invoiceGridHome.Visibility = Visibility.Visible;
            }
            else if (MainVM.isNewRecord && MainVM.SelectedSalesQuote != null)
            {
                //newInvoiceForm.Visibility = Visibility.Visible;
                //computeInvoice();
            }
            else if (MainVM.isPaymentInvoice && MainVM.SelectedSalesInvoice != null)
            {

                //var linqResults = MainVM.PaymentHistory_.Where(x => !(MainVM.SelectedSalesInvoice.invoiceNo_.Equals(x.invoiceNo_)));
                //var observable = new ObservableCollection<PaymentHist>(linqResults);
                //paymentHistoryDg.ItemsSource = observable;
                MainVM.SelectedSalesQuote = MainVM.SalesQuotes.Where(x => x.sqNoChar_.Equals(MainVM.SelectedSalesInvoice.sqNoChar_.ToString())).First();
                paymentDetailsGrid.Visibility = Visibility.Visible;
                paymentDetails.Visibility = Visibility.Visible;
                //computeInvoice();
                amountTb.Value = (double)MainVM.TotalSales;
            }
            else if (MainVM.isNewRecord)
            {
                var linqResults = MainVM.SalesQuotes.Where(x => !(x.status_.Equals("ACCEPTED")));
                var observable = new ObservableCollection<SalesQuote>(linqResults);
                //selectSalesQuote.ItemsSource = observable;
                //transInvoiceGridForm.Visibility = Visibility.Visible;
                //selectSalesQuoteGrid.Visibility = Visibility.Visible;
            }
            MainVM.isNewRecord = false;
            MainVM.isViewHome = false;
            MainVM.isPaymentInvoice = false;
        }

        

        private void newInvoiceBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.isNewRecord = true;
            changeGridView();
        }

    }
}
