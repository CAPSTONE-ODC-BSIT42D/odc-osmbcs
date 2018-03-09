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
    public partial class ucReportService : UserControl
    {
        public ucReportService()
        {
            InitializeComponent();
            DisplayReport();
        }
        private void DisplayReport()
        {
            ReportService.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.ServiceReport.rdlc");
            ReportService.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("ServiceTable", GetService()));
            ReportService.LoadReport(rNames);
            ReportService.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportService.RefreshReport();
        }

        private DataTable GetService()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName, MONTH(ss.dateStarted), YEAR(ss.dateStarted) FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.id = ss.serviceAvailedID INNER JOIN  provinces_t p ON sa.provinceID = p.id, cust_supp_t cs WHERE(s.isDeleted = 0) GROUP BY s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName";

            DataSetReportService.ServiceTableDataTable dSServices = new DataSetReportService.ServiceTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);
       
            return dSServices;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                DisplayReport();
            }
            else
            {
                ReportService.Reset();
            }
        }

    

        private DataTable GetServiceDay()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName, MONTH(ss.dateStarted) AS Expr1, YEAR(ss.dateStarted) AS Expr2 FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.id = ss.serviceAvailedID INNER JOIN  provinces_t p ON sa.provinceID = p.id, cust_supp_t cs WHERE(CURDATE() = ss.dateStarted) AND(s.isDeleted = 0) GROUP BY s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName";

            DataSetReportService.ServiceTableDataTable dSServices = new DataSetReportService.ServiceTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportDayService()
        {
            ReportService.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.ServiceReport.rdlc");
            ReportService.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("ServiceTable", GetServiceDay()));
            ReportService.LoadReport(rNames);
            ReportService.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportService.RefreshReport();
        }
        private DataTable GetServiceWeek()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName, MONTH(ss.dateStarted) AS Expr1, YEAR(ss.dateStarted) AS Expr2 FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.id = ss.serviceAvailedID INNER JOIN provinces_t p ON sa.provinceID = p.id, cust_supp_t cs WHERE(WEEK(ss.dateStarted) ='"+DatePickerWeekSer.SelectedDate.ToString()+"') AND(s.isDeleted = 0) GROUP BY s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName ORDER BY ss.serviceStatus";

            DataSetReportService.ServiceTableDataTable dSServices = new DataSetReportService.ServiceTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportWeekService()
        {
            ReportService.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.ServiceReport.rdlc");
            ReportService.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("ServiceTable", GetServiceWeek()));
            ReportService.LoadReport(rNames);
            ReportService.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportService.RefreshReport();
        }
        private DataTable GetServiceMonth()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text; 
            cmd.CommandText = "SELECT s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName, MONTH(ss.dateStarted) AS Expr1, YEAR(ss.dateStarted) AS Expr2 FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN  service_sched_t ss ON sa.id = ss.serviceAvailedID INNER JOIN provinces_t p ON sa.provinceID = p.id, cust_supp_t cs WHERE(MONTHNAME(ss.dateStarted) = '" + ComboBoxSerMonth.SelectedItem.ToString() + "') AND(s.isDeleted = 0) GROUP BY s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName ORDER BY ss.serviceStatus";

            DataSetReportService.ServiceTableDataTable dSServices = new DataSetReportService.ServiceTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportMonthService()
        {

            ReportService.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.ServiceReport.rdlc");
            ReportService.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("ServiceTable", GetServiceMonth()));
            ReportService.LoadReport(rNames);
            ReportService.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportService.RefreshReport();
        }

        private DataTable GetServiceYear()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName, MONTH(ss.dateStarted) AS Expr1, YEAR(ss.dateStarted) AS Expr2 FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.id = ss.serviceAvailedID INNER JOIN  provinces_t p ON sa.provinceID = p.id, cust_supp_t cs WHERE(YEAR(ss.dateStarted) = '"+ComboBoxYear.SelectedItem.ToString()+"') AND(s.isDeleted = 0) GROUP BY s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName ORDER BY ss.serviceStatus ";

            DataSetReportService.ServiceTableDataTable dSServices = new DataSetReportService.ServiceTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportYearService()
        {
            ReportService.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.ServiceReport.rdlc");
            ReportService.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("ServiceTable", GetServiceYear()));
            ReportService.LoadReport(rNames);
            ReportService.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportService.RefreshReport();
        }
        private DataTable GetServiceRange()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName, MONTH(ss.dateStarted) AS Expr1, YEAR(ss.dateStarted) AS Expr2 FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN  service_sched_t ss ON sa.id = ss.serviceAvailedID INNER JOIN provinces_t p ON sa.provinceID = p.id, cust_supp_t cs WHERE(ss.dateStarted BETWEEN '" + DatePickerStartSer.SelectedDate.ToString()+ "' AND '" + DatePickerEndSer.SelectedDate.ToString() + "') AND(s.isDeleted = 0) GROUP BY s.serviceName, s.serviceDesc, ss.dateStarted, ss.dateEnded, ss.serviceStatus, cs.companyName ORDER BY ss.serviceStatus";

            DataSetReportService.ServiceTableDataTable dSServices = new DataSetReportService.ServiceTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSServices);

            return dSServices;

        }
        private void DisplayReportRangeService()
        {
            ReportService.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.ServiceReport.rdlc");
            ReportService.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("ServiceTable", GetServiceRange()));
            ReportService.LoadReport(rNames);
            ReportService.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportService.RefreshReport();
        }

        private void ComboBoxSerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object SELECTEDINDEX = ComboBoxSerFilter.SelectedIndex;
            if (SELECTEDINDEX.Equals(0))
            {

                DisplayReportDayService();
                WeekDate.Visibility = Visibility.Hidden;
                ComboBoxYear.Visibility = Visibility.Hidden;
                ComboBoxSerMonth.Visibility = Visibility.Hidden;
                monthSer.Visibility = Visibility.Hidden;
                YearSer.Visibility = Visibility.Hidden;
                DatePickerStartSer.Visibility = Visibility.Hidden;
                DatePickerEndSer.Visibility = Visibility.Hidden;
                StartDateSer.Visibility = Visibility.Hidden;
                EndDateSer.Visibility = Visibility.Hidden;
                DatePickerWeekSer.Visibility = Visibility.Hidden;



            }
            if (SELECTEDINDEX.Equals(1))
            {

                DisplayReportWeekService();
                ComboBoxYear.Visibility = Visibility.Hidden;
                ComboBoxSerMonth.Visibility = Visibility.Hidden;
                monthSer.Visibility = Visibility.Hidden;
                YearSer.Visibility = Visibility.Hidden;
                DatePickerStartSer.Visibility = Visibility.Hidden;
                DatePickerEndSer.Visibility = Visibility.Hidden;
                StartDateSer.Visibility = Visibility.Hidden;
                EndDateSer.Visibility = Visibility.Hidden;
                DatePickerWeekSer.Visibility = Visibility.Visible;


            }
            if (SELECTEDINDEX.Equals(2))
            {
                WeekDate.Visibility = Visibility.Hidden;
                DatePickerWeekSer.Visibility = Visibility.Hidden;
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
                WeekDate.Visibility = Visibility.Hidden;
                DatePickerWeekSer.Visibility = Visibility.Hidden;
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
                WeekDate.Visibility = Visibility.Hidden;
                DatePickerWeekSer.Visibility = Visibility.Hidden;
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
            DisplayReportMonthService();
        }

        private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportYearService();
        }

        private void DatePickerStartSer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportRangeService();
        }

        private void DatePickerEndSer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportRangeService();
        }

        private void DatePickerWeekSer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportWeekService();
        }
    }
}
