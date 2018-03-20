using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ucInvoicePaymentHist.xaml
    /// </summary>
    public partial class ucInvoicePaymentHist : UserControl
    {
        public ucInvoicePaymentHist()
        {
            InitializeComponent();
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        private bool validationError = false;

        public event EventHandler SaveCloseOtherButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseOtherButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler ReceivePaymentButtonClicked;
        protected virtual void OnReceivePaymentButtonCliked(RoutedEventArgs e)
        {
            var handler = ReceivePaymentButtonClicked;
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

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && MainVM.SelectedSalesInvoice!=null)
            {
                refreshDataGrid();
                computeInvoice();

                
            }
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
                    MainVM.SelectedSalesInvoice.PaymentHist_.Add(new PaymentT() { SIpaymentID_ = int.Parse(dr["SIpaymentID"].ToString()), SIpaymentDate_ = paymentDate, SIpaymentAmount_ = decimal.Parse(dr["SIpaymentAmount"].ToString()), invoiceNo_ = int.Parse(dr["invoiceNo"].ToString()), SIpaymentMethod_ = dr["SIpaymentMethod"].ToString(), SIcheckNo_ = dr["SIcheckNo"].ToString() });
                }
                dbCon.Close();
            }
        }

        private void receivePaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            OnReceivePaymentButtonCliked(e);
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
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
                    MainVM.VatableSale += totalPric;
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

                    MainVM.VatableSale += totalAmount;
                }

                MainVM.TotalSalesNoVat = Math.Round(MainVM.VatableSale, 2);

                MainVM.VatAmount = (MainVM.VatableSale * ((decimal)0.12));

                MainVM.TotalSales = MainVM.VatableSale + MainVM.VatAmount;
                MainVM.Balance = MainVM.TotalSales;



                decimal totalRec = (from ph in MainVM.SelectedSalesInvoice.PaymentHist_
                                    select ph.SIpaymentAmount_).Sum();

                MainVM.Balance -= (Math.Round(totalRec, 2));
                if (MainVM.Balance == 0)
                    receivePaymentBtn.IsEnabled = false;
                else
                    receivePaymentBtn.IsEnabled = true;
            }

        }

        private void printReceiptBtn_Click(object sender, RoutedEventArgs e)
        {
            OnPrintReceipt(e);
        }

        private void clearCheckBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
