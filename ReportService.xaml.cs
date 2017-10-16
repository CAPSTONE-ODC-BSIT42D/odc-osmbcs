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

            cmd.CommandText = "select s.serviceID, s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus From Services_t s JOIN services_availed_t sa ON s.serviceID = sa.serviceID JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar WHERE (s.isDeleted = 0) order by ss.serviceStatus";

            DataSet1.DataTable1DataTable dSServices = new DataSet1.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);
       
            return dSServices;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReportService.RefreshReport();
        }

    

        private DataTable GetServiceDay()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceID, s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar WHERE(s.isDeleted = 0) AND(Day(ss.dateStarted) = Day(CURDATE())) ORDER BY ss.serviceStatus";

            DataSet1.DataTable1DataTable dSServices = new DataSet1.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportDay()
        {
            ReportService.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetServiceDay()));
            ReportService.LocalReport.ReportEmbeddedResource = "prototype2.Report3.rdlc";
            ReportService.RefreshReport();
        }
        private DataTable GetServiceWeek()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceID, s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar WHERE(s.isDeleted = 0) and YEARWEEK(ss.dateStarted, 1) = YEARWEEK(CURDATE(), 1) ORDER BY ss.serviceStatus";

            DataSet1.DataTable1DataTable dSServices = new DataSet1.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportWeek()
        {
            ReportService.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetServiceWeek()));
            ReportService.LocalReport.ReportEmbeddedResource = "prototype2.Report3.rdlc";
            ReportService.RefreshReport();
        }
        private DataTable GetServiceMonth()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceID, s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar WHERE(s.isDeleted = 0) AND(MONTH(ss.dateStarted) = '"+ ComboBoxSerMonth.SelectedItem.ToString()+ "') ORDER BY ss.serviceStatus";

            DataSet1.DataTable1DataTable dSServices = new DataSet1.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportMonth()
        {
            ReportService.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetServiceMonth()));
            ReportService.LocalReport.ReportEmbeddedResource = "prototype2.Report3.rdlc";
            ReportService.RefreshReport();
        }

        private DataTable GetServiceYear()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceID, s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar WHERE(s.isDeleted = 0) AND(YEAR(ss.dateStarted) = '"+ComboBoxYear.SelectedItem.ToString()+"') ORDER BY ss.serviceStatus";

            DataSet1.DataTable1DataTable dSServices = new DataSet1.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportYear()
        {
            ReportService.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetServiceYear()));
            ReportService.LocalReport.ReportEmbeddedResource = "prototype2.Report3.rdlc";
            ReportService.RefreshReport();
        }
        private DataTable GetServiceRange()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT s.serviceID, s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar WHERE(s.isDeleted = 0) AND(ss.dateStarted BETWEEN '" + DatePickerStartSer.SelectedDate.ToString()+ "' AND '" + DatePickerEndSer.SelectedDate.ToString() + "') ORDER BY ss.serviceStatus";

            DataSet1.DataTable1DataTable dSServices = new DataSet1.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportRange()
        {
            ReportService.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetServiceRange()));
            ReportService.LocalReport.ReportEmbeddedResource = "prototype2.Report3.rdlc";
            ReportService.RefreshReport();
        }

        private void ComboBoxSerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object SELECTEDINDEX = ComboBoxSerFilter.SelectedIndex;
            if (SELECTEDINDEX.Equals(0))
            {

                DisplayReportDay();
                ComboBoxYear.Visibility = Visibility.Hidden;
                ComboBoxSerMonth.Visibility = Visibility.Hidden;
                monthSer.Visibility = Visibility.Hidden;
                YearSer.Visibility = Visibility.Hidden;
                DatePickerStartSer.Visibility = Visibility.Hidden;
                DatePickerEndSer.Visibility = Visibility.Hidden;
                StartDateSer.Visibility = Visibility.Hidden;
                EndDateSer.Visibility = Visibility.Hidden;



            }
            if (SELECTEDINDEX.Equals(1))
            {
                DisplayReportWeek();
                ComboBoxYear.Visibility = Visibility.Hidden;
                ComboBoxSerMonth.Visibility = Visibility.Hidden;
                monthSer.Visibility = Visibility.Hidden;
                YearSer.Visibility = Visibility.Hidden;
                DatePickerStartSer.Visibility = Visibility.Hidden;
                DatePickerEndSer.Visibility = Visibility.Hidden;
                StartDateSer.Visibility = Visibility.Hidden;
                EndDateSer.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(2))
            {
                
                ComboBoxYear.Visibility = Visibility.Hidden;
                ComboBoxSerMonth.Visibility = Visibility.Visible;
                monthSer.Visibility = Visibility.Visible;
                YearSer.Visibility = Visibility.Hidden;
                DatePickerStartSer.Visibility = Visibility.Hidden;
                DatePickerEndSer.Visibility = Visibility.Hidden;
                StartDateSer.Visibility = Visibility.Hidden;
                EndDateSer.Visibility = Visibility.Hidden;

            }
            if (SELECTEDINDEX.Equals(3))
            {
                ComboBoxYear.Visibility = Visibility.Visible;
                ComboBoxSerMonth.Visibility = Visibility.Hidden;
                monthSer.Visibility = Visibility.Hidden;
                YearSer.Visibility = Visibility.Visible;
                DatePickerStartSer.Visibility = Visibility.Hidden;
                DatePickerEndSer.Visibility = Visibility.Hidden;
                StartDateSer.Visibility = Visibility.Hidden;
                EndDateSer.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(4))
            {
                ComboBoxYear.Visibility = Visibility.Hidden;
                ComboBoxSerMonth.Visibility = Visibility.Hidden;
                monthSer.Visibility = Visibility.Hidden;
                YearSer.Visibility = Visibility.Hidden;
                DatePickerStartSer.Visibility = Visibility.Visible;
                DatePickerEndSer.Visibility = Visibility.Visible;
                StartDateSer.Visibility = Visibility.Visible;
                EndDateSer.Visibility = Visibility.Visible;

            }
        }

        private void ComboBoxSerMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportMonth();
        }

        private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportYear();
        }

        private void DatePickerStartSer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportRange();
        }

        private void DatePickerEndSer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportRange();
        }
    }
}
