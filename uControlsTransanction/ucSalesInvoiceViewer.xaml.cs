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

           
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesInvoice.rdlc");
            SalesInvoiceViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSalesInvoice", GetSales()));

            SalesInvoiceViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSIAmount", GetSalesAmount()));
            SalesInvoiceViewer.LoadReport(rNames);
            SalesInvoiceViewer.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            SalesInvoiceViewer.RefreshReport();
           
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
                cmd.CommandText = "SELECT         si.withholdingTax, TRUNCATE(IFNULL(sq.discountPercent, 0), 2) AS LESS_DISCOUNT, TRUNCATE(TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100)) * IFNULL(ia.itemQnty, 0)     + IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) *TRUNCATE(IFNULL(sq.VAT, 0) / 100, 2), 2) AS ADD_VAT_IF_INCLUSIVE, TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc,   0) / 100)) * IFNULL(ia.itemQnty, 0) + IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) -TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100)) * IFNULL(ia.itemQnty, 0)   + IFNULL(sa.totalCost, 0), 2) * TRUNCATE(IFNULL(sq.discountPercent, 0) / 100, 2) + TRUNCATE(TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100)) * IFNULL(ia.itemQnty, 0) + IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) * TRUNCATE(IFNULL(sq.VAT, 0) / 100, 2), 2) AS TOTAL_AMOUNT FROM sales_quote_t sq LEFT OUTER JOIN    items_availed_t ia ON sq.sqNoChar = ia.sqNoChar LEFT OUTER JOIN  item_t i ON i.ID = ia.itemID LEFT OUTER JOIN    markup_hist_t mh ON mh.itemID = ia.itemID INNER JOIN  cust_supp_t cs ON cs.companyID = sq.custID INNER JOIN  provinces_t p ON p.id = cs.companyProvinceID LEFT OUTER JOIN  services_availed_t sa ON sa.sqNoChar = sq.sqNoChar LEFT OUTER JOIN   services_t s ON s.serviceID = sa.serviceID LEFT OUTER JOIN  fees_per_transaction_t ft ON ft.servicesAvailedID = sa.id INNER JOIN   sales_invoice_t si ON si.sqNoChar = sq.sqNoChar WHERE(si.invoiceNo =  '" + result + "') ";

            else
                cmd.CommandText = "SELECT        si.withholdingTax,  TRUNCATE(IFNULL(sq.discountPercent, 0), 2) AS LESS_DISCOUNT, TRUNCATE(TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100)) * IFNULL(ia.itemQnty, 0)     + IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) *TRUNCATE(IFNULL(sq.VAT, 0) / 100, 2), 2) AS ADD_VAT_IF_INCLUSIVE, TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc,   0) / 100)) * IFNULL(ia.itemQnty, 0) + IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) -TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100)) * IFNULL(ia.itemQnty, 0)   + IFNULL(sa.totalCost, 0), 2) * TRUNCATE(IFNULL(sq.discountPercent, 0) / 100, 2) + TRUNCATE(TRUNCATE((IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100)) * IFNULL(ia.itemQnty, 0) + IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) * TRUNCATE(IFNULL(sq.VAT, 0) / 100, 2), 2) AS TOTAL_AMOUNT FROM sales_quote_t sq LEFT OUTER JOIN    items_availed_t ia ON sq.sqNoChar = ia.sqNoChar LEFT OUTER JOIN  item_t i ON i.ID = ia.itemID LEFT OUTER JOIN    markup_hist_t mh ON mh.itemID = ia.itemID INNER JOIN  cust_supp_t cs ON cs.companyID = sq.custID INNER JOIN  provinces_t p ON p.id = cs.companyProvinceID LEFT OUTER JOIN  services_availed_t sa ON sa.sqNoChar = sq.sqNoChar LEFT OUTER JOIN   services_t s ON s.serviceID = sa.serviceID LEFT OUTER JOIN  fees_per_transaction_t ft ON ft.servicesAvailedID = sa.id INNER JOIN   sales_invoice_t si ON si.sqNoChar = sq.sqNoChar WHERE(si.invoiceNo = '" + MainVM.SelectedSalesInvoice.invoiceNo_ + "')";
           

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
                cmd.CommandText = "SELECT        si.invoiceNo, si.custID, si.sqNoChar, si.dateOfIssue, si.termsDays, si.dueDate, si.paymentStatus, si.vat, si.sc_pwd_discount, si.withholdingTax, si.notes, sq.sqNoChar AS Expr1, cs.companyID, sq.termsDP,    s.servicePrice, s.serviceDesc, i.itemDescr, cs.companyName, cs.busStyle, cs.taxNumber, cs.companyAddInfo, cs.companyAddress, cs.companyCity, cs.companyProvinceID, cs.companyPostalCode,   cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.repTitle, cs.repLName, cs.repFName, cs.repMInitial, cs.repEmail, cs.repTelephone, cs.repMobile, cs.companyType, cs.isDeleted, i.itemName,   s.serviceName, TRUNCATE(IFNULL(sq.discountPercent, 0) / 100, 2) AS LESS_DISCOUNT, TRUNCATE(IFNULL(sq.VAT, 0), 2) AS IF_VAT_INCLUSIVE, TRUNCATE(IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0)     * (IFNULL(mh.markupPerc, 0) / 100), 2) AS UNIT_PRICE, ia.itemQnty, TRUNCATE(IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100), 2) * IFNULL(ia.itemQnty, 0) AS total_item,    TRUNCATE(IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) AS total_service FROM sales_quote_t sq LEFT OUTER JOIN  items_availed_t ia ON sq.sqNoChar = ia.sqNoChar LEFT OUTER JOIN   item_t i ON i.ID = ia.itemID LEFT OUTER JOIN  markup_hist_t mh ON mh.itemID = ia.itemID INNER JOIN    cust_supp_t cs ON cs.companyID = sq.custID INNER JOIN   provinces_t p ON p.id = cs.companyProvinceID LEFT OUTER JOIN  services_availed_t sa ON sa.sqNoChar = sq.sqNoChar LEFT OUTER JOIN services_t s ON s.serviceID = sa.serviceID LEFT OUTER JOIN  fees_per_transaction_t ft ON ft.servicesAvailedID = sa.id INNER JOIN    sales_invoice_t si ON si.sqNoChar = sq.sqNoChar  where si.invoiceno='" + result + "'";
            else
                cmd.CommandText = "SELECT        si.invoiceNo, si.custID, si.sqNoChar, si.dateOfIssue, si.termsDays, si.dueDate, si.paymentStatus, si.vat, si.sc_pwd_discount, si.withholdingTax, si.notes, sq.sqNoChar AS Expr1, cs.companyID, sq.termsDP,    s.servicePrice, s.serviceDesc, i.itemDescr, cs.companyName, cs.busStyle, cs.taxNumber, cs.companyAddInfo, cs.companyAddress, cs.companyCity, cs.companyProvinceID, cs.companyPostalCode,   cs.companyEmail, cs.companyTelephone, cs.companyMobile, cs.repTitle, cs.repLName, cs.repFName, cs.repMInitial, cs.repEmail, cs.repTelephone, cs.repMobile, cs.companyType, cs.isDeleted, i.itemName,   s.serviceName, TRUNCATE(IFNULL(sq.discountPercent, 0) / 100, 2) AS LESS_DISCOUNT, TRUNCATE(IFNULL(sq.VAT, 0), 2) AS IF_VAT_INCLUSIVE, TRUNCATE(IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0)     * (IFNULL(mh.markupPerc, 0) / 100), 2) AS UNIT_PRICE, ia.itemQnty, TRUNCATE(IFNULL(ia.unitPrice, 0) + IFNULL(ia.unitPrice, 0) * (IFNULL(mh.markupPerc, 0) / 100), 2) * IFNULL(ia.itemQnty, 0) AS total_item,    TRUNCATE(IFNULL(sa.totalCost, 0) + IFNULL(ft.feeValue, 0), 2) AS total_service FROM sales_quote_t sq LEFT OUTER JOIN  items_availed_t ia ON sq.sqNoChar = ia.sqNoChar LEFT OUTER JOIN   item_t i ON i.ID = ia.itemID LEFT OUTER JOIN  markup_hist_t mh ON mh.itemID = ia.itemID INNER JOIN    cust_supp_t cs ON cs.companyID = sq.custID INNER JOIN   provinces_t p ON p.id = cs.companyProvinceID LEFT OUTER JOIN  services_availed_t sa ON sa.sqNoChar = sq.sqNoChar LEFT OUTER JOIN services_t s ON s.serviceID = sa.serviceID LEFT OUTER JOIN  fees_per_transaction_t ft ON ft.servicesAvailedID = sa.id INNER JOIN    sales_invoice_t si ON si.sqNoChar = sq.sqNoChar  where si.invoiceno='" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";


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
