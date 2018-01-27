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
    public partial class ucReportItem : UserControl
    {
        public ucReportItem()
        {
            InitializeComponent();
            DisplayReport();
        }
        private void DisplayReport()
        {
            ReportItem.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetItem()));
            ReportItem.LocalReport.ReportEmbeddedResource = "prototype2.Report1.rdlc";
            ReportItem.RefreshReport();
        }

        private DataTable GetItem()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "select i.itemCode, i.itemName, i.costPrice, po.orderDate from item_t i JOIN po_line_t p ON i.itemCode = p.itemNo JOIN purchase_order_t po ON p.PONumChar = po.PONumChar";

            DataSet1.item_tDataTable dSItem = new DataSet1.item_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ReportItem.RefreshReport();
        }
        private void DisplayReportItemDay()
        {
            ReportItem.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetItemDay()));
            ReportItem.LocalReport.ReportEmbeddedResource = "prototype2.Report1.rdlc";
            ReportItem.RefreshReport();
        }

        private DataTable GetItemDay()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.costPrice, i.itemUnit, po.orderDate FROM item_t i INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(Day(po.orderDate) = Day(CURDATE()))";

            DataSet1.item_tDataTable dSItem = new DataSet1.item_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportItemWeek()
        {
            ReportItem.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetItemWeek()));
            ReportItem.LocalReport.ReportEmbeddedResource = "prototype2.Report1.rdlc";
            ReportItem.RefreshReport();
        }

        private DataTable GetItemWeek()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.costPrice, i.itemUnit, po.orderDate FROM item_t i INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(YEARWEEK(po.orderDate, 1) = YEARWEEK(CURDATE(), 1))";

            DataSet1.item_tDataTable dSItem = new DataSet1.item_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private void DisplayReportItemMonth()
        {
            ReportItem.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetItemMonth()));
            ReportItem.LocalReport.ReportEmbeddedResource = "prototype2.Report1.rdlc";
            ReportItem.RefreshReport();
        }

        private DataTable GetItemMonth()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.costPrice, i.itemUnit, po.orderDate FROM item_t i INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN  purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(MONTH(po.orderDate) = '"+ ComboBoxItemMonth.SelectedItem.ToString()+ "')";

            DataSet1.item_tDataTable dSItem = new DataSet1.item_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportItemYear()
        {
            ReportItem.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetItemYear()));
            ReportItem.LocalReport.ReportEmbeddedResource = "prototype2.Report1.rdlc";
            ReportItem.RefreshReport();
        }

        private DataTable GetItemYear()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.costPrice, i.itemUnit, po.orderDate FROM item_t i INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(YEAR(po.orderDate) = '"+ ComboBoxItemYear .SelectedItem.ToString()+ "')";

            DataSet1.item_tDataTable dSItem = new DataSet1.item_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportItemRange()
        {
            ReportItem.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetItemRange()));
            ReportItem.LocalReport.ReportEmbeddedResource = "prototype2.Report1.rdlc";
            ReportItem.RefreshReport();
        }

        private DataTable GetItemRange()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        i.itemCode, i.itemName, i.costPrice, i.itemUnit, po.orderDate FROM item_t i INNER JOIN po_line_t p ON i.itemCode = p.itemNo INNER JOIN purchase_order_t po ON p.PONumChar = po.PONumChar WHERE(i.isDeleted = 0) AND(po.orderDate BETWEEN '"+DatePickerItemStart.SelectedDate.ToString()+"' AND '"+DatePickerItemEnd.SelectedDate.ToString()+"')";

            DataSet1.item_tDataTable dSItem = new DataSet1.item_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void ComboBoxItemFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object SELECTEDINDEX = ComboBoxItemFilter.SelectedIndex;
            if (SELECTEDINDEX.Equals(0))
            {
                DisplayReportItemDay();

                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;



            }
            if (SELECTEDINDEX.Equals(1))
            {
                DisplayReportItemWeek();
                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(2))
            {

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
            DisplayReportItemMonth();
        }

        private void ComboBoxItemYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportItemYear();
        }

        private void DatePickerItemStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportItemRange();
        }

        private void DatePickerItemEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportItemRange();
        }
    }
}
