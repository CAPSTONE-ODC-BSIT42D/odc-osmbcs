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
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System.Data;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ucSalesQuoteViewer : UserControl
    {
        public ucSalesQuoteViewer()
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

    private void DisplayReport()
        {


            ucReportViewerSalesQuote.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesQuote.rdlc");
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSales_Quote", GetSalesQuote()));
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetServiceSQ", GetSalesQuote()));
            ucReportViewerSalesQuote.LoadReport(rNames);
            ucReportViewerSalesQuote.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewerSalesQuote.RefreshReport();
        }


        private DataTable GetSalesQuoteSer()
        {


            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if (MainVM.SelectedSalesQuote != null)
            {
                cmd.CommandText = "SELECT        sq.sqNoChar, sq.dateOfIssue, sq.custID, sq.quoteSubject, sq.priceNote, sq.deliveryDate, sq.estDelivery, sq.validityDays, sq.validityDate, sq.otherTerms, sq.VAT, sq.vatIsExcluded, sq.paymentIsLanded,   sq.paymentCurrency, sq.status, sq.termsDays, sq.termsDP, sq.discountPercent, sq.surveyReportDoc, sq.additionalNote, sq.isDeleted, cs.companyID, cs.companyName, cs.busStyle, cs.taxNumber,    cs.companyAddInfo, cs.companyAddress, cs.companyCity, cs.companyProvinceID, cs.companyPostalCode, cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.repTitle, cs.repLName, cs.repFName,   cs.repMInitial, cs.repEmail, cs.repTelephone, cs.repMobile, cs.companyType, cs.isDeleted AS Expr1, s.serviceID, s.serviceName, p.provinceName, sa.totalCost + ft.feeValue AS Expr2, sa.totalCost FROM            sales_quote_t sq INNER JOIN  cust_supp_t cs ON cs.companyID = sq.custID INNER JOIN   services_availed_t sa ON sa.sqNoChar = sq.sqNoChar INNER JOIN  services_t s ON s.serviceID = sa.serviceID INNER JOIN  fees_per_transaction_t ft ON ft.servicesAvailedID = sa.id INNER JOIN   provinces_t p ON p.id = cs.companyProvinceID WHERE        (sq.sqNoChar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "') ";
                DatasetSales_Quote.SQServiceDataTable dSItem = new DatasetSales_Quote.SQServiceDataTable();

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
                mySqlDa.Fill(dSItem);

                return dSItem;
            }


            return null;
        }

        private DataTable GetSalesQuote()
        {


            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if(MainVM.SelectedSalesQuote != null)
            {
                cmd.CommandText = "select sq.dateofissue, sq.sqnochar, c.companyName, c.companyaddress, c.companycity, sq.quotesubject, repTitle,repFName,repLName, repminitial, i.id,s.serviceid,i.itemname, s.servicename, i.itemdescr, s.servicedesc, ia.itemqnty, ia.unitprice, mh.markupperc, s.serviceprice, sa.totalcost, f.feevalue, sq.paymentislanded, sq.termsdp, sq.TERMSDAYs, sq.estDelivery from sales_quote_t sq inner join cust_supp_t c on sq.custid = c.companyid inner join items_availed_t ia on ia.sqnochar = sq.sqnochar inner join item_t i on i.id = ia.itemid inner join markup_hist_t mh on i.id= mh.itemid inner join services_availed_t sa on sq.sqnochar =sa.sqnochar inner join services_t s on sa.serviceid =s.serviceid inner join fees_per_transaction_t f on sa.id = f.servicesavailedid  where sq.sqnochar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "'";
                DatasetSales_Quote.Sales_QuoteDataTable dSItem = new DatasetSales_Quote.Sales_QuoteDataTable();

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
                mySqlDa.Fill(dSItem);

                return dSItem;
            }


            return null;
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                DisplayReport();
            }
            else
                ucReportViewerSalesQuote.Reset();
        }
    }
}