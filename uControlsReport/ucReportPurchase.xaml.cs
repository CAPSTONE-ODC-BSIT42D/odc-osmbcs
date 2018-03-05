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
    public partial class ucReportPurchase : UserControl
    {
        public ucReportPurchase()
        {
            InitializeComponent();
            DisplayReport();
        }
        private void DisplayReport()
        {
            ReportPurchase.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetReportPurchase", GetPurchase()));
            ReportPurchase.LoadReport(rNames);
            ReportPurchase.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportPurchase.RefreshReport();
        }

        private DataTable GetPurchase()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost AS Expr1, SUM(io.totalCost) AS Expr2, SUM(sa.totalCost) AS Expr3, MONTHNAME(po.orderDate) AS Expr4, YEAR(po.orderDate) AS Expr5 FROM services_availed_t sa INNER JOIN services_t s ON sa.serviceID = s.serviceID INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar, cust_supp_t cs INNER JOIN purchase_order_t po ON cs.companyID = po.suppID, items_ordered_t io INNER JOIN item_t i ON io.itemID = i.ID WHERE(i.isDeleted = 0) GROUP BY po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost";

            DataSetReportPurchase.DataTable1DataTable dSPurchase = new DataSetReportPurchase.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void DisplayReportSalesDay()
        {
            ReportPurchase.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetReportPurchase", GetPurchaseDay()));
            ReportPurchase.LoadReport(rNames);
            ReportPurchase.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportPurchase.RefreshReport();
        }

        private DataTable GetPurchaseDay()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost AS Expr1, SUM(io.totalCost) AS Expr2, SUM(sa.totalCost) AS Expr3, MONTHNAME(po.orderDate) AS Expr4, YEAR(po.orderDate) AS Expr5 FROM services_availed_t sa INNER JOIN services_t s ON sa.serviceID = s.serviceID INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar, cust_supp_t cs INNER JOIN purchase_order_t po ON cs.companyID = po.suppID, items_ordered_t io INNER JOIN  item_t i ON io.itemID = i.ID WHERE(CURDATE() = po.orderDate) AND(i.isDeleted = 0) GROUP BY po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost";

            DataSetReportPurchase.DataTable1DataTable dSPurchase = new DataSetReportPurchase.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void DisplayReportPurchaseWeek()
        {
            ReportPurchase.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetReportPurchase", GetPurchaseWeek()));
            ReportPurchase.LoadReport(rNames);
            ReportPurchase.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportPurchase.RefreshReport();
        }

        private DataTable GetPurchaseWeek()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost AS Expr1, SUM(io.totalCost) AS Expr2, SUM(sa.totalCost) AS Expr3, MONTHNAME(po.orderDate) AS Expr4, YEAR(po.orderDate) AS Expr5 FROM services_availed_t sa INNER JOIN services_t s ON sa.serviceID = s.serviceID INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar, cust_supp_t cs INNER JOIN  purchase_order_t po ON cs.companyID = po.suppID, items_ordered_t io INNER JOIN   item_t i ON io.itemID = i.ID WHERE(WEEK(po.orderDate) = '" + DatePickerWeekSales.SelectedDate.ToString() +"') AND(i.isDeleted = 0) GROUP BY po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost";

            DataSetReportPurchase.DataTable1DataTable dSPurchase = new DataSetReportPurchase.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void DisplayReportPurchaseYear()
        {
            ReportPurchase.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetReportPurchase", GetPurchaseYear()));
            ReportPurchase.LoadReport(rNames);
            ReportPurchase.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportPurchase.RefreshReport();
        }

        private DataTable GetPurchaseYear()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost AS Expr1, SUM(io.totalCost) AS Expr2, SUM(sa.totalCost) AS Expr3, MONTHNAME(po.orderDate) AS Expr4, YEAR(po.orderDate) AS Expr5 FROM services_availed_t sa INNER JOIN services_t s ON sa.serviceID = s.serviceID INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar, cust_supp_t cs INNER JOIN purchase_order_t po ON cs.companyID = po.suppID, items_ordered_t io INNER JOIN item_t i ON io.itemID = i.ID WHERE(YEAR(po.orderDate) = '" + ComboBoxYearSales.SelectedItem.ToString() + "') AND(i.isDeleted = 0) GROUP BY po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost";

            DataSetReportPurchase.DataTable1DataTable dSPurchase = new DataSetReportPurchase.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void DisplayReportPurchaseMonth()
        {
            ReportPurchase.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetReportPurchase", GetPurchaseMonth()));
            ReportPurchase.LoadReport(rNames);
            ReportPurchase.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportPurchase.RefreshReport();
        }

        private DataTable GetPurchaseMonth()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost AS Expr1, SUM(io.totalCost) AS Expr2, SUM(sa.totalCost) AS Expr3, MONTHNAME(po.orderDate) AS Expr4, YEAR(po.orderDate) AS Expr5 FROM services_availed_t sa INNER JOIN services_t s ON sa.serviceID = s.serviceID INNER JOIN  sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar, cust_supp_t cs INNER JOIN purchase_order_t po ON cs.companyID = po.suppID, items_ordered_t io INNER JOIN item_t i ON io.itemID = i.ID WHERE(MONTHNAME(po.orderDate) = '"+ ComboBoxMonthSales.SelectedItem.ToString() + "') AND(i.isDeleted = 0) GROUP BY po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost";

            DataSetReportPurchase.DataTable1DataTable dSPurchase = new DataSetReportPurchase.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void DisplayReportPurchaseRange()
        {
            ReportPurchase.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSetReportPurchase", GetPurchaseRange()));
            ReportPurchase.LoadReport(rNames);
            ReportPurchase.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportPurchase.RefreshReport();
        }

        private DataTable GetPurchaseRange()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost AS Expr1, SUM(io.totalCost) AS Expr2, SUM(sa.totalCost) AS Expr3, MONTHNAME(po.orderDate) AS Expr4, YEAR(po.orderDate) AS Expr5 FROM services_availed_t sa INNER JOIN services_t s ON sa.serviceID = s.serviceID INNER JOIN sales_quote_t sq ON sa.sqNoChar = sq.sqNoChar, cust_supp_t cs INNER JOIN purchase_order_t po ON cs.companyID = po.suppID, items_ordered_t io INNER JOIN  item_t i ON io.itemID = i.ID WHERE(po.orderDate BETWEEN '" + DatePickerStartSales.SelectedDate.ToString() +"' AND '"+DatePickerEndSales.SelectedDate.ToString()+"')  AND(i.isDeleted = 0) GROUP BY po.orderDate, i.itemName, io.totalCost, s.serviceName, sa.totalCost ";

            DataSetReportPurchase.DataTable1DataTable dSPurchase = new DataSetReportPurchase.DataTable1DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReportPurchase.RefreshReport();
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
                DatePickerWeekSales.Visibility = Visibility.Hidden;
                Weeksales.Visibility = Visibility.Hidden;

            }
            if (SELECTEDINDEX.Equals(1))

            {
                DisplayReportPurchaseWeek();
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Hidden;
                DatePickerStartSales.Visibility = Visibility.Hidden;
                DatePickerEndSales.Visibility = Visibility.Hidden;
                startsales.Visibility = Visibility.Hidden;
                EndSales.Visibility = Visibility.Hidden;
                DatePickerWeekSales.Visibility = Visibility.Visible;
                Weeksales.Visibility = Visibility.Visible;

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
                DatePickerWeekSales.Visibility = Visibility.Hidden;
                Weeksales.Visibility = Visibility.Hidden;
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
                DatePickerWeekSales.Visibility = Visibility.Hidden;
                Weeksales.Visibility = Visibility.Hidden;

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
                DatePickerWeekSales.Visibility = Visibility.Hidden;
                Weeksales.Visibility = Visibility.Hidden;
            }
        }

        private void ComboBoxMonthSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportPurchaseMonth();
        }

        private void ComboBoxYearSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportPurchaseYear();
        }
        private void DatePickerStartSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportPurchaseRange();
        }
        private void DatePickerEndSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportPurchaseRange();
        }

        private void DatePickerWeekSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportPurchaseWeek();
        }
    }
}
