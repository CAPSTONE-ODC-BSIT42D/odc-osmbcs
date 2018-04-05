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
                ComboBoxFilter.SelectedIndex = 0;
         
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

            cmd.CommandText = "SELECT        itemName, SUM(TOTAL_DISCOUNTED_ITEM) AS Expr1 FROM sales_report_item_view WHERE(MONTHNAME(_date) = '" + ((ComboBoxItem)ComboBoxMonth.SelectedItem).Content.ToString() + "') GROUP BY itemName  ";
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

            cmd.CommandText = " SELECT        serviceName, SUM(total_discounted_service) AS Expr1 FROM sales_report_service_view WHERE(MONTHNAME(_date) = '" + ((ComboBoxItem)ComboBoxMonth.SelectedItem).Content.ToString() + "') group by servicename ";
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

            cmd.CommandText = "SELECT        itemName, SUM(TOTAL_DISCOUNTED_ITEM) AS Expr1 FROM sales_report_item_view WHERE(YEAR(_date) = '" + ComboBoxYear.SelectedValue.ToString() + "')  GROUP BY itemName  ";

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

            cmd.CommandText = " SELECT        serviceName, SUM(total_discounted_service) AS Expr1 FROM sales_report_service_view WHERE(YEAR(_date) = '" + ComboBoxYear.SelectedValue.ToString() + "') group by servicename ";

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

            cmd.CommandText = "SELECT        itemName, SUM(TOTAL_DISCOUNTED_ITEM) AS Expr1 FROM sales_report_item_view WHERE(_date BETWEEN '" + DateStart.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AND '" + DateEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + "') GROUP BY itemName  ";

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

            cmd.CommandText = " SELECT        serviceName, SUM(total_discounted_service) AS Expr1 FROM sales_report_service_view WHERE(_date BETWEEN  '" + DateStart.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AND '" + DateEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + "') group by servicename ";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (ComboBoxFilter.SelectedIndex == 0)
                {
                    Monthly.Visibility = Visibility.Visible;
                    Yearly.Visibility = Visibility.Collapsed;
                    Range.Visibility = Visibility.Collapsed;


                }
                if (ComboBoxFilter.SelectedIndex == 1)
                {
                    Monthly.Visibility = Visibility.Collapsed;
                    Yearly.Visibility = Visibility.Visible;
                    Range.Visibility = Visibility.Collapsed;


                }
                if (ComboBoxFilter.SelectedIndex == 2)
                {
                    Monthly.Visibility = Visibility.Collapsed;
                    Yearly.Visibility = Visibility.Collapsed;
                    Range.Visibility = Visibility.Visible;

                }
                if (ComboBoxFilter.SelectedIndex == 3)
                {


                }
                if (ComboBoxFilter.SelectedIndex == 4)
                {

                }
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

        private void DatePickerItemStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DatePickerItemEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
          
         
        }

        private void DatePickerItemWeek_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
          
           
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            bool validationError = false;
            if (DateStart.SelectedDate.HasValue && DateEnd.SelectedDate.HasValue)
            {
                foreach (var element in Range.Children)
                {
                    if (element is DatePicker)
                    {
                        BindingExpression expression = ((DatePicker)element).GetBindingExpression(DatePicker.SelectedDateProperty);
                        if (expression != null)
                        {
                            if (Validation.GetHasError((DatePicker)element))
                                validationError = true;
                        }
                    }
                }
                if(!validationError)
                    DisplayReportSalesRange();
                
            }


            else
                MessageBox.Show("Select the data range");
        }
    }
}
