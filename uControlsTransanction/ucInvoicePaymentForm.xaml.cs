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
        
        DateTime dateOfIssue = new DateTime();
        public event EventHandler SaveClosePaymentForm;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveClosePaymentForm;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler PrintReceipt;
        protected virtual void OnPrintReceipt(RoutedEventArgs e)
        {
            var handler = PrintReceipt;
            if (handler != null)
                handler(this, e);
        }

        private void savePrintBtn_Click(object sender, RoutedEventArgs e)
        {
            saveDataToDb();
            OnSaveCloseButtonClicked(e);
            OnPrintReceipt(e);
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

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && MainVM.SelectedSalesInvoice != null)
            {
                MainVM.SelectedCustomerSupplier = (from cust in MainVM.Customers
                                                   where cust.CompanyID == MainVM.SelectedSalesInvoice.custID_
                                                   select cust).FirstOrDefault();
                computeInvoice();
            }
        }

        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            string query;
            decimal totalRec = (from ph in MainVM.SelectedSalesInvoice.PaymentHist_
                                select ph.SIpaymentAmount_).Sum();
            
            if (totalRec +amountTb.Value < MainVM.TotalSales)
            {
                query = "UPDATE `sales_invoice_t` SET paymentStatus = '" + "PARTIALLY PAID" + "' WHERE invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";
                dbCon.insertQuery(query, dbCon.Connection);
            }
            else if (totalRec + amountTb.Value >= MainVM.TotalSales)
            {
                query = "UPDATE `sales_invoice_t` SET paymentStatus = '" + "FULLY PAID" + "' WHERE invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";
                dbCon.insertQuery(query, dbCon.Connection);
            }


            if(paymentMethodCb.SelectedIndex == 0)
            {
                query = "INSERT INTO `odc_db`.`si_payment_t` " +
    "(`SIpaymentAmount`,`SIpaymentMethod`,`SIcheckNo`,`SIpaymentStatus`,`invoiceNo`) " +
    "VALUES " +
    "('" + amountTb.Value + "','" +
    paymentMethodCb.SelectedValue + "','" +
    checkNoTb.Text + "','" +
        "PAID" + "','" +
    MainVM.SelectedSalesInvoice.invoiceNo_ + "')";
                dbCon.insertQuery(query, dbCon.Connection);
            }
            else
            {
                query = "INSERT INTO `odc_db`.`si_payment_t` " +
    "(`SIpaymentAmount`,`SIpaymentMethod`,`SIcheckNo`,`SIpaymentStatus`,`invoiceNo`) " +
    "VALUES " +
    "('" + amountTb.Value + "','" +
    paymentMethodCb.SelectedValue + "','" +
    checkNoTb.Text + "','" +
    "PENDING" + "','" +
    MainVM.SelectedSalesInvoice.invoiceNo_ + "')";
                dbCon.insertQuery(query, dbCon.Connection);
            }

            refreshDataGrid();
            
        }

        void refreshDataGrid()
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                MainVM.SelectedSalesInvoice.PaymentHist_.Clear();
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
                    MainVM.SelectedSalesInvoice.PaymentHist_.Add(new PaymentT() { SIpaymentID_ = int.Parse(dr["SIpaymentID"].ToString()), SIpaymentDate_ = paymentDate, SIpaymentAmount_ = decimal.Parse(dr["SIpaymentAmount"].ToString()), invoiceNo_ = int.Parse(dr["invoiceNo"].ToString()), SIpaymentMethod_ = dr["SIpaymentMethod"].ToString(), SIpaymentStatus_ = dr["SIpaymentStatus"].ToString(), SIcheckNo_ = dr["SIcheckNo"].ToString() });
                }
                dbCon.Close();
            }
        }

        void computeInvoice()
        {
            if (MainVM.SelectedSalesInvoice != null)
            {
                MainVM.VatableSale = 0;
                MainVM.TotalSalesWithOutDp = 0;

                MainVM.SelectedCustomerSupplier = (from cust in MainVM.Customers
                                                   where cust.CompanyID == MainVM.SelectedSalesInvoice.custID_
                                                   select cust).FirstOrDefault();
                MainVM.SelectedSalesQuote = MainVM.SalesQuotes.Where(x => x.sqNoChar_.Equals(MainVM.SelectedSalesInvoice.sqNoChar_)).FirstOrDefault();
                var invoiceprod = from ai in MainVM.AvailedItems
                                  where ai.SqNoChar.Equals(MainVM.SelectedSalesInvoice.sqNoChar_)
                                  select ai;
                var invoiceserv = from aser in MainVM.AvailedServices
                                  where aser.SqNoChar.Equals(MainVM.SelectedSalesInvoice.sqNoChar_)
                                  select aser;
                foreach (AvailedItem ai in invoiceprod)
                {
                    var markupPrice = from itm in MainVM.MarkupHist
                                      where itm.ItemID == ai.ItemID
                                      && itm.DateEffective <= MainVM.SelectedSalesQuote.dateOfIssue_
                                      select itm;
                    decimal totalPric = ai.ItemQty * (ai.UnitPrice + (ai.UnitPrice / 100 * markupPrice.Last().MarkupPerc));
                    MainVM.VatableSale += Math.Round(totalPric, 2);
                }

                foreach (AvailedService aserv in invoiceserv)
                {
                    MainVM.SelectedProvince = (from prov in MainVM.Provinces
                                               where prov.ProvinceID == aserv.ProvinceID
                                               select prov).FirstOrDefault();
                    MainVM.SelectedRegion = (from rg in MainVM.Regions
                                             where rg.RegionID == MainVM.SelectedProvince.RegionID
                                             select rg).FirstOrDefault();

                    var service = from serv in MainVM.ServicesList
                                  where serv.ServiceID == aserv.ServiceID
                                  select serv;

                    decimal totalFee = (from af in aserv.AdditionalFees
                                        select af.FeePrice).Sum();
                    decimal totalAmount = aserv.TotalCost + totalFee;
                    
                    MainVM.VatableSale += Math.Round(totalAmount, 2);
                }

                MainVM.TotalSalesNoVat = Math.Round(MainVM.VatableSale, 2);

                MainVM.VatAmount = (MainVM.VatableSale * ((decimal)0.12));
                MainVM.VatAmount = Math.Round(MainVM.VatAmount, 2);

                MainVM.TotalSales = Math.Round(MainVM.VatableSale + MainVM.VatAmount, 2);
                MainVM.Balance = MainVM.TotalSales;



                decimal totalRec = (from ph in MainVM.SelectedSalesInvoice.PaymentHist_
                                 select ph.SIpaymentAmount_).Sum();

                MainVM.Balance -= (Math.Round(totalRec, 2));

                int dpPerc = (from sq in MainVM.SalesQuotes
                                             where MainVM.SelectedSalesInvoice.sqNoChar_.Equals(sq.sqNoChar_)
                                             select sq.termsDP_).FirstOrDefault();
                if (MainVM.Balance == MainVM.TotalSales)
                    amountTb.Value = MainVM.TotalSales - (MainVM.TotalSales / 100 * dpPerc);

                else
                    amountTb.Value = MainVM.Balance;
            }

        }
        
        private void paymentMethodCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {

                if (paymentMethodCb.SelectedIndex == 1)
                {
                    checkTb.Visibility = Visibility.Visible;
                }
                else
                    checkTb.Visibility = Visibility.Collapsed;
            }
        }
    }
}
