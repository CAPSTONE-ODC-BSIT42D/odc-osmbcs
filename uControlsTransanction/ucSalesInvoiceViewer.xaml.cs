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

            ucSalesInvoiceViewer.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesInvoice.rdlc");
            ucSalesInvoiceViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetSalesInvoice", GetSales()));
            ucSalesInvoiceViewer.LoadReport(rNames);
            ucSalesInvoiceViewer.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucSalesInvoiceViewer.RefreshReport();
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
                cmd.CommandText = "select c.companyname, si.tin, c.companyaddress, c.companycity, si.busstyle, curdate() as datetoday, si.termsdays, i.itemname, s.servicename,s.serviceprice, i.itemdescr, s.servicedesc, u.unitName, ia.itemqnty, ia.unitprice, mh.markupPerc, sa.totalCost, f.feeValue, si.invoiceno, si.vat, si.withholdingtax, si.notes FROM sales_invoice_t si inner join cust_supp_t c on si.custid = c.companyid inner join sales_quote_t sq on sq.sqnochar = si.sqnochar inner join items_availed_t ia on ia.sqnochar =sq.sqnochar inner join item_t i on i.id =ia.itemid inner join unit_t u on u.id = i.unitid inner join markup_hist_t mh on i.id =mh.itemid inner join services_availed_t sa on sa.sqnochar = sq.sqnochar inner join services_t s on s.serviceid=sa.serviceid inner join fees_per_transaction_t f on f.servicesavailedid = sa.id where si.invoiceno='" + result + "'";
            else
                cmd.CommandText = "select c.companyname, si.tin, c.companyaddress, c.companycity, si.busstyle, curdate() as datetoday, si.termsdays, i.itemname, s.servicename,s.serviceprice, i.itemdescr, s.servicedesc, u.unitName, ia.itemqnty, ia.unitprice, mh.markupPerc, sa.totalCost, f.feeValue, si.invoiceno, si.vat, si.withholdingtax, si.notes FROM sales_invoice_t si inner join cust_supp_t c on si.custid = c.companyid inner join sales_quote_t sq on sq.sqnochar = si.sqnochar inner join items_availed_t ia on ia.sqnochar =sq.sqnochar inner join item_t i on i.id =ia.itemid inner join unit_t u on u.id = i.unitid inner join markup_hist_t mh on i.id =mh.itemid inner join services_availed_t sa on sa.sqnochar = sq.sqnochar inner join services_t s on s.serviceid=sa.serviceid inner join fees_per_transaction_t f on f.servicesavailedid = sa.id where si.invoiceno='" + MainVM.SelectedSalesInvoice.invoiceNo_ + "'";


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
                ucSalesInvoiceViewer.Reset();
            }
        }
        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }
    }
}
