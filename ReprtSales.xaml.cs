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
        private void DisplayReportSalesDay()
        {
            ReportSales.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetSalesDay()));
            ReportSales.LocalReport.ReportEmbeddedResource = "prototype2.Report4.rdlc";
            ReportSales.RefreshReport();
        }

        private DataTable GetSalesDay()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.itemUnit, i.salesPrice, po.orderDate, s.serviceID, s.serviceName, s.serviceDesc, CONCAT(sa.address, sa.city) AS Location, ss.dateStarted, ss.dateEnded, sa.totalCost FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar INNER JOIN items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN item_t i ON i.itemCode = ia.itemCode INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(s.isDeleted = 0) AND(Day(ss.dateStarted) = Day(CURDATE()))";

            DataSet1.DataTable3DataTable dSSales = new DataSet1.DataTable3DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSSales);

            return dSSales;


        }
        private void DisplayReportSalesWeek()
        {
            ReportSales.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetSalesWeek()));
            ReportSales.LocalReport.ReportEmbeddedResource = "prototype2.Report4.rdlc";
            ReportSales.RefreshReport();
        }

        private DataTable GetSalesWeek()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.itemUnit, i.salesPrice, po.orderDate, s.serviceID, s.serviceName, s.serviceDesc, CONCAT(sa.address, sa.city) AS Location, ss.dateStarted, ss.dateEnded, sa.totalCost FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar INNER JOIN items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN item_t i ON i.itemCode = ia.itemCode INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(s.isDeleted = 0) AND(YEARWEEK(ss.dateStarted, 1) = YEARWEEK(CURDATE(), 1))";

            DataSet1.DataTable3DataTable dSSales = new DataSet1.DataTable3DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSSales);

            return dSSales;


        }
        private void DisplayReportSalesYear()
        {
            ReportSales.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetSalesYear()));
            ReportSales.LocalReport.ReportEmbeddedResource = "prototype2.Report4.rdlc";
            ReportSales.RefreshReport();
        }

        private DataTable GetSalesYear()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.itemUnit, i.salesPrice, po.orderDate, s.serviceID, s.serviceName, s.serviceDesc, CONCAT(sa.address, sa.city) AS Location, ss.dateStarted, ss.dateEnded, sa.totalCost FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar INNER JOIN items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN item_t i ON i.itemCode = ia.itemCode INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(s.isDeleted = 0) AND(YEAR(ss.dateStarted) ='" + ComboBoxYearSales.SelectedItem.ToString() + "')";

            DataSet1.DataTable3DataTable dSSales = new DataSet1.DataTable3DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSSales);

            return dSSales;


        }
        private void DisplayReportSalesMonth()
        {
            ReportSales.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetSalesMonth()));
            ReportSales.LocalReport.ReportEmbeddedResource = "prototype2.Report4.rdlc";
            ReportSales.RefreshReport();
        }

        private DataTable GetSalesMonth()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.itemUnit, i.salesPrice, po.orderDate, s.serviceID, s.serviceName, s.serviceDesc, CONCAT(sa.address, sa.city) AS Location, ss.dateStarted, ss.dateEnded, sa.totalCost FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar INNER JOIN items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN item_t i ON i.itemCode = ia.itemCode INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(s.isDeleted = 0) AND(YEAR(ss.dateStarted) = '"+ ComboBoxMonthSales.SelectedItem.ToString() + "')";

            DataSet1.DataTable3DataTable dSSales = new DataSet1.DataTable3DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSSales);

            return dSSales;


        }
        private void DisplayReportSalesRange()
        {
            ReportSales.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetSalesRange()));
            ReportSales.LocalReport.ReportEmbeddedResource = "prototype2.Report4.rdlc";
            ReportSales.RefreshReport();
        }

        private DataTable GetSalesRange()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.itemUnit, i.salesPrice, po.orderDate, s.serviceID, s.serviceName, s.serviceDesc, CONCAT(sa.address, sa.city) AS Location, ss.dateStarted, ss.dateEnded, sa.totalCost FROM services_t s INNER JOIN services_availed_t sa ON s.serviceID = sa.serviceID INNER JOIN service_sched_t ss ON sa.tableNoChar = ss.serviceSchedNoChar INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar INNER JOIN items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN item_t i ON i.itemCode = ia.itemCode INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(s.isDeleted = 0) AND(ss.dateStarted BETWEEN '" + DatePickerStartSales.SelectedDate.ToString() +"' AND '"+DatePickerEndSales.SelectedDate.ToString()+"')";

            DataSet1.DataTable3DataTable dSSales = new DataSet1.DataTable3DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSSales);

            return dSSales;


        }
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReportSales.RefreshReport();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object SELECTEDINDEX = ComboBoxSalesFilter.SelectedIndex;
            if (SELECTEDINDEX.Equals(0))
            {
                DisplayReportSalesDay();
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Hidden;
                DatePickerStartSales.Visibility = Visibility.Hidden;
                DatePickerEndSales.Visibility = Visibility.Hidden;
                startsales.Visibility = Visibility.Hidden;
                EndSales.Visibility = Visibility.Hidden;



            }
            if (SELECTEDINDEX.Equals(1))

            {
                DisplayReportSalesWeek();
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Hidden;
                DatePickerStartSales.Visibility = Visibility.Hidden;
                DatePickerEndSales.Visibility = Visibility.Hidden;
                startsales.Visibility = Visibility.Hidden;
                EndSales.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(2))
            {

                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Visible;
                MonthSales.Visibility = Visibility.Visible;
                YearSales.Visibility = Visibility.Hidden;
                DatePickerStartSales.Visibility = Visibility.Hidden;
                DatePickerEndSales.Visibility = Visibility.Hidden;
                startsales.Visibility = Visibility.Hidden;
                EndSales.Visibility = Visibility.Hidden;

            }
            if (SELECTEDINDEX.Equals(3))
            {
                ComboBoxYearSales.Visibility = Visibility.Visible;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Visible;
                DatePickerStartSales.Visibility = Visibility.Hidden;
                DatePickerEndSales.Visibility = Visibility.Hidden;
                startsales.Visibility = Visibility.Hidden;
                EndSales.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(4))
            {
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Hidden;
                DatePickerStartSales.Visibility = Visibility.Visible;
                DatePickerEndSales.Visibility = Visibility.Visible;
                startsales.Visibility = Visibility.Visible;
                EndSales.Visibility = Visibility.Visible;

            }
        }

        private void ComboBoxMonthSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesMonth();
        }

        private void ComboBoxYearSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesYear();
        }
        private void DatePickerStartSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesRange();
        }
        private void DatePickerEndSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesRange();
        }
    }
}
