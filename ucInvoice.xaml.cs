using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }
        Document document;
        private void invoiceNext_Click(object sender, RoutedEventArgs e)
        {
            if (newInvoiceForm.IsVisible)
            {
                newInvoiceForm.Visibility = Visibility.Collapsed;
                documentViewer.Visibility = Visibility.Visible;
            }
            else if (documentViewer.IsVisible)
            {
                salesInvoiceToMemory();
                InvoiceDocument df = new InvoiceDocument();
                document = df.CreateDocument("SalesQuote", "asdsadsa");
                string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
                documentViewer.pagePreview.Ddl = ddl;
            }
                
        }

        private void invoiceBack_Click(object sender, RoutedEventArgs e)
        {
            if (newInvoiceForm.IsVisible)
            {
                newInvoiceForm.Visibility = Visibility.Collapsed;
                documentViewer.Visibility = Visibility.Visible;
                OnSaveCloseButtonClicked(e);
            }
            else if (documentViewer.IsVisible)
            {

            }
        }

        

        void salesInvoiceToMemory()
        {
            string noChar = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var numbs = "01234567890123456789";

            var random = new Random();


            if (MainVM.SelectedCustomerSupplier.CompanyName.Length > 5)
            {
                noChar = MainVM.SelectedCustomerSupplier.CompanyName.Trim().Substring(0, 5).ToUpper();

            }
            else
                noChar = MainVM.SelectedCustomerSupplier.CompanyName.Trim().ToUpper();
            string stringChars = "";
            for (int i = 0; i < 4; i++)
            {
                if (!(i > 3))
                {
                    stringChars += chars[random.Next(chars.Length)];
                }
            }
            stringChars += "SI";
            stringChars += MainVM.SalesInvoice.Count + 1;
            stringChars += "-";
            foreach (char c in noChar)
            {
                stringChars += c;
            }
            stringChars += "-";
            stringChars += DateTime.Now.ToString("yyyy-MM-dd");

            MainVM.SelectedSalesInvoice = (new SalesInvoice() { });
        }
    }
}
