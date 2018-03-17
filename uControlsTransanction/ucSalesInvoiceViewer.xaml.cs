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
    public partial class ucSalesInvoiceViewer : UserControl
    {
        public ucSalesInvoiceViewer()
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

            SalesInvoiceViewer.DataSources.Clear();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesInvoice.rdlc");
            SalesInvoiceViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSalesInvoice", GetSales()));
            SalesInvoiceViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSIAmount", GetSalesAmount()));
            SalesInvoiceViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetItemService", GetSalesItemService()));
            SalesInvoiceViewer.LoadReport(rNames);
            SalesInvoiceViewer.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            SalesInvoiceViewer.RefreshReport();
           
        }
        private DataTable GetSalesItemService()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            string query = "SELECT LAST_INSERT_ID();";
            string result = dbCon.selectScalar(query, dbCon.Connection).ToString();

            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if (MainVM.SelectedSalesInvoice == null)
                cmd.CommandText = "SELECT        invoiceNo, dateOfIssue, dueDate, notes, sqNoChar, quoteSubject, priceNote, deliveryDate, estDelivery, validityDays, validityDate, otherTerms, vat, paymentCurrency, termsDays, termsDP, discountPercent,   additionalNote, busStyle, taxNumber, CompanyAddInfo, companyAddress, companyCity, companyPostalCode, companyEmail, companyTelephone, companyMobile, repTitle, repLname, repFName, repMInitial,   repEmail, repMobile, companyName, ID, DESCRIPTION, itemName, itemDescr, UNIT_PRICE, itemQnty, total_item FROM            si_view_indiv WHERE(invoiceNo = '" + result + "') ";

            else
                cmd.CommandText = "SELECT        invoiceNo, dateOfIssue, dueDate, notes, sqNoChar, quoteSubject, priceNote, deliveryDate, estDelivery, validityDays, validityDate, otherTerms, vat, paymentCurrency, termsDays, termsDP, discountPercent,   additionalNote, busStyle, taxNumber, CompanyAddInfo, companyAddress, companyCity, companyPostalCode, companyEmail, companyTelephone, companyMobile, repTitle, repLname, repFName, repMInitial,   repEmail, repMobile, companyName, ID, DESCRIPTION, itemName, itemDescr, UNIT_PRICE, itemQnty, total_item FROM            si_view_indiv WHERE(invoiceNo ='" + MainVM.SelectedSalesInvoice.invoiceNo_ + "') ";


            DataSetSalesInvoice.si_view_indivDataTable dSItem = new DataSetSalesInvoice.si_view_indivDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private DataTable GetSalesAmount()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            string query = "SELECT LAST_INSERT_ID();";
            string result = dbCon.selectScalar(query, dbCon.Connection).ToString();
           
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if (MainVM.SelectedSalesInvoice == null)
                cmd.CommandText = "SELECT        invoiceNo, sqNoChar, discountPercent, SUM(total_item) AS TOTAL, SUM(total_item) * IFNULL(vat, 0) / 100 AS VAT, SUM(total_item) + SUM(total_item) * IFNULL(vat, 0) / 100 AS TOTAL_WITH_VAT FROM si_view_indiv WHERE(invoiceNo = '" + result + "') GROUP BY invoiceNo ";

            else
                cmd.CommandText = "SELECT        invoiceNo, sqNoChar, discountPercent, SUM(total_item) AS TOTAL, SUM(total_item) * IFNULL(vat, 0) / 100 AS VAT, SUM(total_item) + SUM(total_item) * IFNULL(vat, 0) / 100 AS TOTAL_WITH_VAT FROM si_view_indiv WHERE(invoiceNo ='" + MainVM.SelectedSalesInvoice.invoiceNo_ + "') GROUP BY invoiceNo ";
           

            DataSetSalesInvoice.sales_Invoice_tDataTable dSItem = new DataSetSalesInvoice.sales_Invoice_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;
         
        }
      
        private DataTable GetSales()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            string query = "SELECT LAST_INSERT_ID();";
            string result = dbCon.selectScalar(query, dbCon.Connection).ToString();

            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
            if (MainVM.SelectedSalesInvoice == null)
                cmd.CommandText = "SELECT        si.invoiceNo, si.custID, si.sqNoChar, si.dateOfIssue, si.termsDays, si.dueDate, si.paymentStatus, si.vat, si.sc_pwd_discount, si.withholdingTax, si.notes, cust_supp_t.companyID, cust_supp_t.companyName,    cust_supp_t.busStyle, cust_supp_t.taxNumber, cust_supp_t.companyAddInfo, cust_supp_t.companyAddress, cust_supp_t.companyCity, cust_supp_t.companyProvinceID, cust_supp_t.companyPostalCode,   cust_supp_t.companyEmail, cust_supp_t.companyTelephone, cust_supp_t.companyMobile, cust_supp_t.repTitle, cust_supp_t.repLName, cust_supp_t.repFName, cust_supp_t.repMInitial, cust_supp_t.repEmail,    cust_supp_t.repTelephone, cust_supp_t.repMobile, cust_supp_t.companyType, cust_supp_t.isDeleted, sq.sqNoChar AS Expr1, sq.dateOfIssue AS Expr2, sq.custID AS Expr3, sq.quoteSubject, sq.priceNote,   sq.deliveryDate, sq.estDelivery, sq.validityDays, sq.validityDate, sq.otherTerms, sq.VAT AS Expr4, sq.vatIsExcluded, sq.paymentIsLanded, sq.paymentCurrency, sq.status, sq.termsDays AS Expr5, sq.termsDP,   sq.discountPercent, sq.surveyReportDoc, sq.additionalNote, sq.isDeleted AS Expr6 FROM            sales_quote_t sq INNER JOIN  cust_supp_t ON sq.custID = cust_supp_t.companyID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar WHERE(si.invoiceNo = '" + result + "')";
            else
                cmd.CommandText = "SELECT        si.invoiceNo, si.custID, si.sqNoChar, si.dateOfIssue, si.termsDays, si.dueDate, si.paymentStatus, si.vat, si.sc_pwd_discount, si.withholdingTax, si.notes, cust_supp_t.companyID, cust_supp_t.companyName,    cust_supp_t.busStyle, cust_supp_t.taxNumber, cust_supp_t.companyAddInfo, cust_supp_t.companyAddress, cust_supp_t.companyCity, cust_supp_t.companyProvinceID, cust_supp_t.companyPostalCode,   cust_supp_t.companyEmail, cust_supp_t.companyTelephone, cust_supp_t.companyMobile, cust_supp_t.repTitle, cust_supp_t.repLName, cust_supp_t.repFName, cust_supp_t.repMInitial, cust_supp_t.repEmail,    cust_supp_t.repTelephone, cust_supp_t.repMobile, cust_supp_t.companyType, cust_supp_t.isDeleted, sq.sqNoChar AS Expr1, sq.dateOfIssue AS Expr2, sq.custID AS Expr3, sq.quoteSubject, sq.priceNote,   sq.deliveryDate, sq.estDelivery, sq.validityDays, sq.validityDate, sq.otherTerms, sq.VAT AS Expr4, sq.vatIsExcluded, sq.paymentIsLanded, sq.paymentCurrency, sq.status, sq.termsDays AS Expr5, sq.termsDP,   sq.discountPercent, sq.surveyReportDoc, sq.additionalNote, sq.isDeleted AS Expr6 FROM            sales_quote_t sq INNER JOIN  cust_supp_t ON sq.custID = cust_supp_t.companyID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar WHERE(si.invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "')";


            DataSetSalesInvoice.SalesInvoiceDataTable dSItem = new DataSetSalesInvoice.SalesInvoiceDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                DisplayReport();
            }
            else
            {

            }
        }
        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
    }
}
