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


namespace prototype2.uControlsMaintenance
{
    /// <summary>
    /// Interaction logic for ucInvoicePaymentForm.xaml
    /// </summary>
    public partial class ucInvoicePaymentForm : UserControl
    {
        public ucInvoicePaymentForm()
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

        private void savePrintBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            string query = "SELECT invoiceNo FROM sales_invoice_t ORDER BY invoiceNo DESC LIMIT 1;";
            MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
            DataSet fromDb = new DataSet();
            DataTable fromDbTable = new DataTable();
            dataAdapter.Fill(fromDb, "t");
            fromDbTable = fromDb.Tables["t"];
            MainVM.SalesQuotes.Clear();

            foreach (DataRow dr in fromDbTable.Rows)
            {
                query = "INSERT INTO `odc_db`.`si_payment_t` " +
                "(`SIpaymentAmount`,`SIpaymentMethod`,`SIcheckNo`,`invoiceNo`) " +
                "VALUES " +
                "('" + amountTb.Value + "','" +
                paymentMethodCb.SelectedValue + "','" +
                checkNoTb.Text + "','" +
                MainVM.SelectedSalesInvoice.invoiceNo_ + "')";
                dbCon.insertQuery(query, dbCon.Connection);
            }

            query = "SELECT SIpaymentID FROM si_payment_t ORDER BY SIpaymentID DESC LIMIT 1;";
            dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
            fromDb = new DataSet();
            fromDbTable = new DataTable();
            dataAdapter.Fill(fromDb, "t");
            fromDbTable = fromDb.Tables["t"];
            foreach (DataRow dr in fromDbTable.Rows)
            {
                MainVM.PaymentID = dr["SIpaymentID"].ToString();
            }
            decimal total = 0;
            foreach (PaymentT pt in MainVM.PaymentList_)
            {
                if (pt.invoiceNo_.ToString().Equals(MainVM.SelectedSalesInvoice.invoiceNo_))
                    total += pt.SIpaymentAmount_;
            }
            //decimal total = linqResults.Select(x => x.SIpaymentAmount_).Sum();

            if (((double)total + amountTb.Value) < (double)MainVM.TotalSales)
            {
                query = "UPDATE `sales_invoice_t` SET paymentStatus = '" + "PARTIALLY PAID" + "' WHERE invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";
                dbCon.insertQuery(query, dbCon.Connection);
            }
            else if (((double)total + amountTb.Value) >= (double)MainVM.TotalSales)
            {
                query = "UPDATE `sales_invoice_t` SET paymentStatus = '" + "FULLY PAID" + "' WHERE invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";
                dbCon.insertQuery(query, dbCon.Connection);
            }
            //receiptVeiwer.Visibility = Visibility.Visible;
            OnSaveCloseButtonClicked(e);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
    }
}
