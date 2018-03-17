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

            
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                DisplayReport();
         
            }
        }
        private void DisplayReport()
        {

            ReportSales.DataSources.Clear();


            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");

            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesItemTable", GetItem()));
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesServiceTable", GetSalesSer()));
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

            cmd.CommandText = "SELECT        itemName, SUM(TOTAL_DISCOUNTED_ITEM) AS Expr1 FROM sales_report_item_view GROUP BY itemName";
           
            DatasetReportSales.SalesItemDataTable dSItem = new DatasetReportSales.SalesItemDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private DataTable GetSalesSer()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

  
            cmd.CommandText = "SELECT        serviceName, SUM(total_discounted_service) AS Expr1 FROM sales_report_service_view GROUP BY serviceName ";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }



     

     
 

        private void DisplayReportSalesMonth()
        {

            ReportSales.DataSources.Clear();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");

            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesItemTable", GetItemMonth()));
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesServiceTable", GetSerMonth()));
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

            cmd.CommandText = "SELECT        itemName, SUM(TOTAL_DISCOUNTED_ITEM) AS Expr1 FROM sales_report_item_view WHERE(MONTHNAME(_date) = '" + ((ComboBoxItem)ComboBoxItemMonth.SelectedItem).Content.ToString() + "') GROUP BY itemName  ";
            DatasetReportSales.SalesItemDataTable dSItem = new DatasetReportSales.SalesItemDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private DataTable GetSerMonth()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = " SELECT        serviceName, SUM(total_discounted_service) AS Expr1 FROM sales_report_service_view WHERE(MONTHNAME(_date) = '" + ((ComboBoxItem)ComboBoxItemMonth.SelectedItem).Content.ToString() + "') group by servicename ";
            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesYear()
        {
            ReportSales.DataSources.Clear();

            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");

            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesItemTable", GetItemYear()));
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesServiceTable", GetSerYear()));
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

            cmd.CommandText = "SELECT        itemName, SUM(TOTAL_DISCOUNTED_ITEM) AS Expr1 FROM sales_report_item_view WHERE(YEAR(_date) = '" + ((ComboBoxItem)ComboBoxItemYear.SelectedItem).Content.ToString() + "')  GROUP BY itemName  ";

            DatasetReportSales.SalesItemDataTable dSItem = new DatasetReportSales.SalesItemDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private DataTable GetSerYear()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = " SELECT        serviceName, SUM(total_discounted_service) AS Expr1 FROM sales_report_service_view WHERE(YEAR(_date) = '" + ((ComboBoxItem)ComboBoxItemYear.SelectedItem).Content.ToString() + "') group by servicename ";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesRange()
        {
            ReportSales.DataSources.Clear();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");

            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesItemTable", GetItemRange()));
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesServiceTable", GetSerRange()));
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

            cmd.CommandText = "SELECT        itemName, SUM(TOTAL_DISCOUNTED_ITEM) AS Expr1 FROM sales_report_item_view WHERE(_date BETWEEN '" + DatePickerItemStart.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AND '" + DatePickerItemEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + "') GROUP BY itemName  ";

            DatasetReportSales.SalesItemDataTable dSItem = new DatasetReportSales.SalesItemDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private DataTable GetSerRange()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = " SELECT        serviceName, SUM(total_discounted_service) AS Expr1 FROM sales_report_service_view WHERE(_date BETWEEN  '" + DatePickerItemStart.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AND '" + DatePickerItemEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + "') group by servicename ";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void ComboBoxItemFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxItemFilter.SelectedIndex == 0)
            {
                ReportSales.Reset();
                ItemWeek.Visibility = Visibility.Hidden;
              
                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Visible;
                MonthItem.Visibility = Visibility.Visible;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;


            }
            if (ComboBoxItemFilter.SelectedIndex == 1)
            {
                ReportSales.Reset();
                ItemWeek.Visibility = Visibility.Hidden;
              
                ComboBoxItemYear.Visibility = Visibility.Visible;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Visible;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;
                Go_ButtonSales.Visibility = Visibility.Hidden;


            }
            if (ComboBoxItemFilter.SelectedIndex == 2)
            {
                ItemWeek.Visibility = Visibility.Hidden;
          
                ComboBoxItemYear.Visibility = Visibility.Hidden;
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Visible;
                DatePickerItemEnd.Visibility = Visibility.Visible;
                ItemStart.Visibility = Visibility.Visible;
                ItemEnd.Visibility = Visibility.Visible;
                Go_ButtonSales.Visibility = Visibility.Visible;

            }
            if (ComboBoxItemFilter.SelectedIndex == 3)
            {
            

            }
            if (ComboBoxItemFilter.SelectedIndex == 4)
            {
             
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
        }

        private void DatePickerItemEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
          
         
        }

        private void DatePickerItemWeek_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
          
           
        }

        private void Go_ButtonSales_Click(object sender, RoutedEventArgs e)
        {
            DisplayReportSalesRange();
        }
    }
}
