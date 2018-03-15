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


            //ucReportViewerSalesQuote.Reset();
            ucReportViewerSalesQuote.DataSources.Clear();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesQuote.rdlc");
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSalesQuote", GetSalesQuote()));
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSQAmount", GetSalesQuoteAmount()));
            ucReportViewerSalesQuote.LoadReport(rNames);
            ucReportViewerSalesQuote.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewerSalesQuote.RefreshReport();
        }

        private DataTable GetSalesQuoteAmount()
        {


            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if (MainVM.SelectedSalesQuote != null)
            {
                cmd.CommandText = "SELECT        sq.sqNoChar, (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100)) * ia.itemQnty + sa.totalCost AS AMOUNT FROM sales_quote_t sq INNER JOIN   cust_supp_t cs ON cs.companyID = sq.custID INNER JOIN   items_availed_t ia ON ia.sqNoChar = ia.sqNoChar INNER JOIN   item_t i ON i.ID = ia.itemID INNER JOIN    markup_hist_t mh ON mh.itemID = i.ID INNER JOIN  provinces_t p ON p.id = cs.companyProvinceID INNER JOIN  services_availed_t sa ON sa.sqNoChar = sq.sqNoChar INNER JOIN   services_t s ON s.serviceID = sa.serviceID WHERE(sq.sqNoChar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "') GROUP BY sq.sqNoChar ";
                DatasetSales_Quote.sales_quote_tDataTable dSItem = new DatasetSales_Quote.sales_quote_tDataTable();

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
                cmd.CommandText = "SELECT sq.*, cs.*, i.id, i.itemName, s.serviceName,s.servicePrice, s.serviceDesc,i.id,s.serviceid,i.itemdescr, TRUNCATE(IFNULL(sq.discountPercent,0)/100,2) AS LESS_DISCOUNT,     TRUNCATE(IFNULL(sq.vat, 0), 2) AS IF_VAT_INCLUSIVE,   TRUNCATE(IFNULL(ia.unitPrice, 0) + (IFNULL(ia.unitPrice, 0)) *   (IFNULL((mh.markupperc), 0) / 100), 2) AS UNIT_PRICE, ia.itemQnty, 	TRUNCATE(IFNULL(ia.unitPrice, 0) + (IFNULL(ia.unitPrice, 0)) *  (IFNULL((mh.markupperc), 0) / 100), 2) *  IFNULL(ia.itemQnty, 0) as total_item,   TRUNCATE(IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) as total_service from sales_quote_t sq left join items_availed_t ia  on sq.sqnochar = ia.sqnochar left join item_t i on i.id = ia.itemId left Join markup_hist_t mh on mh.itemID = ia.itemID JOIN cust_supp_t cs on cs.companyID = sq.custID  join provinces_t p on p.id = cs.companyProvinceID left JOIN services_availed_t sa on sa.sqnochar = sq.sqnochar left JOIN services_t s on s.serviceID = sa.serviceID left join fees_per_transaction_t ft on ft.servicesAvailedID = sa.id  where sq.sqnochar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "' ";
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