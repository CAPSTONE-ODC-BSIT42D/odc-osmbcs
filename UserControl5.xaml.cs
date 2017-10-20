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
    /// Interaction logic for UserControl5.xaml
    /// </summary>
    public partial class UserControl5 : UserControl
    {
        public UserControl5()
        {
            InitializeComponent();
            DisplayReport();
        }

        private void WinFormHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void DisplayReport()
        {
            Contract.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetReceipt()));
            Contract.LocalReport.ReportEmbeddedResource = "prototype2.Report7.rdlc";
            Contract.RefreshReport();
        }

        private DataTable GetReceipt()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT     c.companyName, c.companyAddress, c.companyCity, pv.locProvince, si.invoiceNo, sp.SIpaymentAmount, si.busStyle, sp.SIpaymentMethod, sp.SIcheckNo FROM cust_supp_t c INNER JOIN sales_invoice_t si ON c.companyID = si.custID INNER JOIN si_payment_t sp ON si.invoiceNo = sp.invoiceNo INNER JOIN  provinces_t pv ON pv.locProvinceID = c.companyProvinceID";

            DataSet1.DataTable5DataTable dSReceipt = new DataSet1.DataTable5DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSReceipt);

            return dSReceipt;

        }
    }
}
