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
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class ucReportSales : UserControl
    {
        public ucReportSales()
        {
            InitializeComponent();
            DisplayReport();
            
        }
        private void DisplayReport()
        {
            ReportSales.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesTable", GetItem()));
            ReportSales.LoadReport(rNames);
            ReportSales.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportSales.RefreshReport();

        }

        private DataTable GetItem()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemName, s.serviceName, ia.totalCost, sa.totalCost AS Expr1, si.dateOfIssue, SUM(ia.totalCost) AS Expr2, MONTHNAME(si.dateOfIssue) AS Expr3, YEAR(si.dateOfIssue) AS Expr4, SUM(sa.totalCost) AS Expr5 FROM item_t i INNER JOIN items_availed_t ia ON i.ID = ia.itemID INNER JOIN cust_supp_t cs ON i.supplierID = cs.companyID INNER JOIN sales_invoice_t si ON cs.companyID = si.custID INNER JOIN service_sched_t ss ON si.invoiceNo = ss.invoiceNo INNER JOIN services_availed_t sa ON ss.serviceAvailedID = sa.id INNER JOIN services_t s ON sa.serviceID = s.serviceID WHERE(i.isDeleted = 0) AND(s.isDeleted = 0) GROUP BY i.itemName, s.serviceName, ia.totalCost, sa.totalCost, si.dateOfIssue";

            DatasetReportSales.SalesTableDataTable dSItem = new DatasetReportSales.SalesTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReportSales.RefreshReport();
        }
        private void DisplayReportSalesDay()
        {
            ReportSales.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesTable", GetItemDay()));
            ReportSales.LoadReport(rNames);
            ReportSales.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportSales.RefreshReport();
        }

        private DataTable GetItemDay()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemName, s.serviceName, ia.totalCost, sa.totalCost AS Expr1, si.dateOfIssue, SUM(ia.totalCost) AS Expr2, MONTHNAME(si.dateOfIssue) AS Expr3, YEAR(si.dateOfIssue) AS Expr4, SUM(sa.totalCost)  AS Expr5 FROM item_t i INNER JOIN items_availed_t ia ON i.ID = ia.itemID INNER JOIN cust_supp_t cs ON i.supplierID = cs.companyID INNER JOIN sales_invoice_t si ON cs.companyID = si.custID INNER JOIN service_sched_t ss ON si.invoiceNo = ss.invoiceNo INNER JOIN services_availed_t sa ON ss.serviceAvailedID = sa.id INNER JOIN services_t s ON sa.serviceID = s.serviceID WHERE(CURDATE() = si.dateOfIssue) AND(i.isDeleted = 0) AND(s.isDeleted = 0) GROUP BY i.itemName, s.serviceName, ia.totalCost, sa.totalCost, si.dateOfIssue";

            DatasetReportSales.SalesTableDataTable dSItem = new DatasetReportSales.SalesTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesWeek()
        {
            ReportSales.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesTable", GetItemWeek()));
            ReportSales.LoadReport(rNames);
            ReportSales.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportSales.RefreshReport();
        }

        private DataTable GetItemWeek()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemName, s.serviceName, ia.totalCost, sa.totalCost AS Expr1, si.dateOfIssue, SUM(ia.totalCost) AS Expr2, MONTHNAME(si.dateOfIssue) AS Expr3, YEAR(si.dateOfIssue) AS Expr4, SUM(sa.totalCost)  AS Expr5 FROM item_t i INNER JOIN items_availed_t ia ON i.ID = ia.itemID INNER JOIN  cust_supp_t cs ON i.supplierID = cs.companyID INNER JOIN  sales_invoice_t si ON cs.companyID = si.custID INNER JOIN service_sched_t ss ON si.invoiceNo = ss.invoiceNo INNER JOIN services_availed_t sa ON ss.serviceAvailedID = sa.id INNER JOIN services_t s ON sa.serviceID = s.serviceID WHERE(WEEK(si.dateOfIssue) = '" + DatePickerItemWeek.SelectedDate.ToString() + "') and (i.isDeleted = 0) AND(s.isDeleted = 0) GROUP BY i.itemName, s.serviceName, ia.totalCost, sa.totalCost, si.dateOfIssue";

            DatasetReportSales.SalesTableDataTable dSItem = new DatasetReportSales.SalesTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private void DisplayReportSalesMonth()
        {
            ReportSales.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesTable", GetItemMonth()));
            ReportSales.LoadReport(rNames);
            ReportSales.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportSales.RefreshReport();
        }

        private DataTable GetItemMonth()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemName, s.serviceName, ia.totalCost, sa.totalCost AS Expr1, si.dateOfIssue, SUM(ia.totalCost) AS Expr2, MONTHNAME(si.dateOfIssue) AS Expr3, YEAR(si.dateOfIssue) AS Expr4, SUM(sa.totalCost)   AS Expr5 FROM item_t i INNER JOIN items_availed_t ia ON i.ID = ia.itemID INNER JOIN cust_supp_t cs ON i.supplierID = cs.companyID INNER JOIN sales_invoice_t si ON cs.companyID = si.custID INNER JOIN service_sched_t ss ON si.invoiceNo = ss.invoiceNo INNER JOIN services_availed_t sa ON ss.serviceAvailedID = sa.id INNER JOIN services_t s ON sa.serviceID = s.serviceID WHERE(MONTHNAME(si.dateOfIssue) = '" + ComboBoxItemMonth.SelectedItem.ToString() + "') AND(i.isDeleted = 0) AND(s.isDeleted = 0) GROUP BY i.itemName, s.serviceName, ia.totalCost, sa.totalCost, si.dateOfIssue";
            DatasetReportSales.SalesTableDataTable dSItem = new DatasetReportSales.SalesTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesYear()
        {

            ReportSales.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesTable", GetItemYear()));
            ReportSales.LoadReport(rNames);
            ReportSales.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportSales.RefreshReport();
        }

        private DataTable GetItemYear()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemName, s.serviceName, ia.totalCost, sa.totalCost AS Expr1, si.dateOfIssue, SUM(ia.totalCost) AS Expr2, MONTHNAME(si.dateOfIssue) AS Expr3, YEAR(si.dateOfIssue) AS Expr4, SUM(sa.totalCost)  AS Expr5 FROM item_t i INNER JOIN items_availed_t ia ON i.ID = ia.itemID INNER JOIN cust_supp_t cs ON i.supplierID = cs.companyID INNER JOIN sales_invoice_t si ON cs.companyID = si.custID INNER JOIN service_sched_t ss ON si.invoiceNo = ss.invoiceNo INNER JOIN services_availed_t sa ON ss.serviceAvailedID = sa.id INNER JOIN services_t s ON sa.serviceID = s.serviceID WHERE(YEAR(si.dateOfIssue) =  '" + ComboBoxItemYear.SelectedItem.ToString() + "') AND(i.isDeleted = 0) AND(s.isDeleted = 0) GROUP BY i.itemName, s.serviceName, ia.totalCost, sa.totalCost, si.dateOfIssue";

            DatasetReportSales.SalesTableDataTable dSItem = new DatasetReportSales.SalesTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesRange()
        {
            ReportSales.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesTable", GetItemRange()));
            ReportSales.LoadReport(rNames);
            ReportSales.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportSales.RefreshReport();
        }

        private DataTable GetItemRange()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemName, s.serviceName, ia.totalCost, sa.totalCost AS Expr1, si.dateOfIssue, SUM(ia.totalCost) AS Expr2, MONTHNAME(si.dateOfIssue) AS Expr3, YEAR(si.dateOfIssue) AS Expr4, SUM(sa.totalCost)  AS Expr5 FROM item_t i INNER JOIN items_availed_t ia ON i.ID = ia.itemID INNER JOIN  cust_supp_t cs ON i.supplierID = cs.companyID INNER JOIN sales_invoice_t si ON cs.companyID = si.custID INNER JOIN service_sched_t ss ON si.invoiceNo = ss.invoiceNo INNER JOIN services_availed_t sa ON ss.serviceAvailedID = sa.id INNER JOIN services_t s ON sa.serviceID = s.serviceID WHERE(si.dateOfIssue BETWEEN '" + DatePickerItemStart.SelectedDate.ToString() + "' AND '" + DatePickerItemEnd.SelectedDate.ToString() + "') AND (i.isDeleted = 0) AND(s.isDeleted = 0) GROUP BY i.itemName, s.serviceName, ia.totalCost, sa.totalCost, si.dateOfIssue";

            DatasetReportSales.SalesTableDataTable dSItem = new DatasetReportSales.SalesTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void ComboBoxItemFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object SELECTEDINDEX = ComboBoxItemFilter.SelectedIndex;
            if (SELECTEDINDEX.Equals(0))
            {
                DisplayReportSalesDay();
                ItemWeek.Visibility = Visibility.Hidden;
                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;
                DatePickerItemWeek.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(1))
            {
                DisplayReportSalesWeek();
                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;
                DatePickerItemWeek.Visibility = Visibility.Visible;
                ItemWeek.Visibility = Visibility.Visible;

            }
            if (SELECTEDINDEX.Equals(2))
            {
                ItemWeek.Visibility = Visibility.Hidden;
                DatePickerItemWeek.Visibility = Visibility.Hidden;
                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Visible;
                MonthItem.Visibility = Visibility.Visible;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;

            }
            if (SELECTEDINDEX.Equals(3))
            {
                ItemWeek.Visibility = Visibility.Hidden;
                DatePickerItemWeek.Visibility = Visibility.Hidden;
                ComboBoxItemYear.Visibility = Visibility.Visible;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Visible;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(4))
            {
                ItemWeek.Visibility = Visibility.Hidden;
                DatePickerItemWeek.Visibility = Visibility.Hidden;
                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Visible;
                DatePickerItemEnd.Visibility = Visibility.Visible;
                ItemStart.Visibility = Visibility.Visible;
                ItemEnd.Visibility = Visibility.Visible;

            }
        }

        private void ComboBoxItemMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesMonth();
        }

        private void ComboBoxItemYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesYear();
        }

        private void DatePickerItemStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesRange();
        }

        private void DatePickerItemEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesRange();
        }

        private void DatePickerItemWeek_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportSalesWeek();
        }
    }
}
