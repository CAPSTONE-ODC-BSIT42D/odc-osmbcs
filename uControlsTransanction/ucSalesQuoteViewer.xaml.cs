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
            ucReportViewerSalesQuote.DataSources.Clear();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesQuote.rdlc");
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSalesQuote", GetSalesQuote()));
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSQAmount", GetSalesQuoteAmount()));
            ucReportViewerSalesQuote.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetItemService", GetSalesItemService()));
            ucReportViewerSalesQuote.LoadReport(rNames);
            ucReportViewerSalesQuote.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewerSalesQuote.RefreshReport();
        }
             private DataTable GetSalesItemService()
        {


            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if (MainVM.SelectedSalesQuote != null)
            {
                cmd.CommandText = "       SELECT        sqNoChar, dateOfIssue, quoteSubject, priceNote, deliveryDate, estDelivery, validityDays, validityDate, otherTerms, vat, paymentCurrency, termsDays, termsDP, discountPercent, additionalNote, busStyle,   taxNumber, CompanyAddInfo, companyAddress, companyCity, companyPostalCode, companyEmail, companyTelephone, companyMobile, repTitle, repLname, repFName, repMInitial, repEmail, repMobile,    companyName, ID, DESCRIPTION, itemName, UNIT_PRICE, itemQnty, total_item FROM            sq_view_indiv WHERE(sqNoChar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "')   ";
                DatasetSales_Quote.si_view_indivDataTable dSItem = new DatasetSales_Quote.si_view_indivDataTable();

                MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
                mySqlDa.Fill(dSItem);

                return dSItem;
            }


            return null;
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
                cmd.CommandText = "SELECT        sqNoChar, discountPercent, SUM(total_item) AS TOTAL, SUM(total_item) * IFNULL(vat, 0) / 100 AS VAT, SUM(total_item) + SUM(total_item) * IFNULL(vat, 0) / 100 AS TOTAL_WITH_VAT FROM sq_view_indiv WHERE(sqNoChar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "') GROUP BY sqNoChar  ";
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
                cmd.CommandText = "select * from sales_quote_t, cust_supp_t where custid=companyid and sqnochar = '" + MainVM.SelectedSalesQuote.sqNoChar_ + "' ";
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

        private void ucReportViewerSalesQuote_ReportExport(object sender, Syncfusion.Windows.Reports.ReportExportEventArgs e)
        {
            e.FileName = MainVM.SelectedSalesQuote.sqNoChar_;
        }
    }
}