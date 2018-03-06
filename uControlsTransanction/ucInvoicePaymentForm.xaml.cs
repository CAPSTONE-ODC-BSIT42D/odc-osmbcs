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
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        
        private bool 
            
            
            
            
            
            
            g2 = false;
        
        DateTime dateOfIssue = new DateTime();
        public event EventHandler SaveClosePaymentForm;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveClosePaymentForm;
            if (handler != null)
                handler(this, e);
        }

        private void savePrintBtn_Click(object sender, RoutedEventArgs e)
        {
            saveDataToDb();
            OnSaveCloseButtonClicked(e);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            saveDataToDb();
            OnSaveCloseButtonClicked(e);
        }

        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            string query;
            decimal total = (from ph in MainVM.SelectedSalesInvoice.PaymentHist_
                             select ph.SIpaymentAmount_).Sum();
            decimal totalamount = 0;

            foreach (AvailedItem ai in (from ai in MainVM.AvailedItems
                     where ai.SqNoChar.Equals(MainVM.SelectedSalesInvoice.sqNoChar_)
                     select ai))
            {
                var markupPrice = from itm in MainVM.MarkupHist
                                  where itm.ItemID == ai.ItemID
                                  && itm.DateEffective <= MainVM.SelectedSalesQuote.dateOfIssue_
                                  select itm;
                 totalamount += ai.UnitPrice + (ai.UnitPrice / 100 * markupPrice.Last().MarkupPerc);
            }

            totalamount += (from aser in MainVM.AvailedServices
                            where aser.SqNoChar.Equals(MainVM.SelectedSalesInvoice.sqNoChar_)
                            select aser.TotalCost).Sum();

            if ((total + (decimal)amountTb.Value) < totalamount)
            {
                query = "UPDATE `sales_invoice_t` SET paymentStatus = '" + "PARTIALLY PAID" + "' WHERE invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";
                dbCon.insertQuery(query, dbCon.Connection);
            }
            else if ((total + (decimal)amountTb.Value) >= totalamount)
            {
                query = "UPDATE `sales_invoice_t` SET paymentStatus = '" + "FULLY PAID" + "' WHERE invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";
                dbCon.insertQuery(query, dbCon.Connection);
            }

            query = "INSERT INTO `odc_db`.`si_payment_t` " +
                "(`SIpaymentAmount`,`SIpaymentMethod`,`SIcheckNo`,`invoiceNo`) " +
                "VALUES " +
                "('" + amountTb.Value + "','" +
                paymentMethodCb.SelectedValue + "','" +
                checkNoTb.Text + "','" +
                MainVM.SelectedSalesInvoice.invoiceNo_ + "')";
            dbCon.insertQuery(query, dbCon.Connection);
            refreshDataGrid();
            
        }

        void refreshDataGrid()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM si_payment_t where invoiceNo = " + MainVM.SelectedSalesInvoice.invoiceNo_;
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    DateTime paymentDate = new DateTime();
                    DateTime.TryParse(dr["SIpaymentDate"].ToString(), out paymentDate);
                    MainVM.SelectedSalesInvoice.PaymentHist_.Add(new PaymentT() { SIpaymentID_ = int.Parse(dr["SIpaymentID"].ToString()), SIpaymentDate_ = paymentDate, SIpaymentAmount_ = decimal.Parse(dr["SIpaymentAmount"].ToString()), invoiceNo_ = int.Parse(dr["invoiceNo"].ToString()), SIpaymentMethod_ = dr["SIpaymentMethod"].ToString(), SIcheckNo_ = dr["SIcheckNo"].ToString() });
                }
                dbCon.Close();
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(this.IsVisible && MainVM.SelectedSalesInvoice != null)
            {
                MainVM.SelectedCustomerSupplier = (from cust in MainVM.Customers
                                                   where cust.CompanyID == MainVM.SelectedSalesInvoice.custID_
                                                   select cust).FirstOrDefault();
            }
        }
    }
}
