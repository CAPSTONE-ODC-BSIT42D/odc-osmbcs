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
        private void DisplayReport()
        {


            ucReportViewerSalesQuote.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesQuote.rdlc");
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSales_Quote", GetSalesQuote()));
            ucReportViewerSalesQuote.LoadReport(rNames);
            ucReportViewerSalesQuote.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewerSalesQuote.RefreshReport();
        }

        private DataTable GetSalesQuote()
        {


            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "select sq.dateofissue, sq.sqnochar, c.companyName, c.companyaddress, r.regionName, sq.quotesubject, repTitle,repFName,repLName, repminitial, i.id,s.serviceid,i.itemname, s.servicename, i.itemdescr, s.servicedesc, ia.itemqnty, ia.unitprice, mh.markupperc, s.serviceprice, sa.totalcost, f.feevalue, sq.paymentislanded, sq.termsdp, sq.TERMSDAYs, sq.estDelivery from sales_quote_t sq inner join cust_supp_t c on sq.custid = c.companyid inner join items_availed_t ia on ia.sqnochar = sq.sqnochar inner join item_t i on i.id = ia.itemid inner join markup_hist_t mh on i.id= mh.itemid inner join services_availed_t sa on sq.sqnochar =sa.sqnochar inner join services_t s on sa.serviceid =s.serviceid inner join fees_per_transaction_t f on sa.id = f.servicesavailedid inner join provinces_t p on p.id= c.companyprovinceid inner join regions_t r on r.id = p.regionid where sq.sqnochar = " + MainVM.SelectedSalesQuote.sqNoChar_ + "";

             DatasetSales_Quote.Sales_QuoteDataTable dSItem = new DatasetSales_Quote.Sales_QuoteDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;
        }
    }
}