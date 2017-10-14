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
    /// Interaction logic for UserControl3.xaml
    /// </summary>
    public partial class UserControl3 : UserControl
    {
        public UserControl3()
        {
            InitializeComponent();
            DisplayReport();
        }
        private void DisplayReport()
        {
            ReportSales.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetSales()));
            ReportSales.LocalReport.ReportEmbeddedResource = "prototype2.Report4.rdlc";
            ReportSales.RefreshReport();
        }

        private DataTable GetSales()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT  i.itemCode, i.itemName, i.itemUnit, i.salesPrice, po.orderDate, s.serviceID, s.serviceName, s.serviceDesc, CONCAT(sa.address, sa.city) AS Location, ss.dateStarted, ss.dateEnded, sa.totalCost FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar INNER JOIN items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN item_t i ON i.itemCode = ia.itemCode INNER JOIN  po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar";

            DataSet1.DataTable3DataTable dSSales = new DataSet1.DataTable3DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSSales);

            return dSSales;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReportSales.RefreshReport();
        }
    }
}
