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
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            DisplayReport();
        }
        private void DisplayReport()
        {
            ReportService.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetService()));
            ReportService.LocalReport.ReportEmbeddedResource = "prototype2.Report3.rdlc";
            ReportService.RefreshReport();
        }

        private DataTable GetService()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "select s.serviceID, s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus From Services_t s JOIN services_availed_t sa ON s.serviceID = sa.serviceID JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar order by ss.serviceStatus";

            DataSet1.DataTable1DataTable dSServices = new DataSet1.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReportService.RefreshReport();
        }
    }
}
