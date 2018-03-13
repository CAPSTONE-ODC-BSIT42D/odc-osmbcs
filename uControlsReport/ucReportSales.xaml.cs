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

            ReportSales.Reset();


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

            cmd.CommandText = "SELECT        i.itemName, SUM((IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0) + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) - (IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0)  + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) *(IFNULL(si.sc_pwd_discount, 0) / 100)) AS TOTAL_ITEM FROM sales_quote_t sq INNER join items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN  item_t i ON ia.itemID = i.ID INNER JOIN markup_hist_t mh ON i.ID = mh.itemID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar GROUP BY i.itemName";
           
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

  
            cmd.CommandText = "SELECT        s.serviceName, sa.totalCost + IFNULL(ft.feeValue, 0) AS TOTAL_SERVICE FROM sales_quote_t sq INNER JOIN   services_availed_t sa ON sq.sqNoChar = sa.sqNoChar INNER JOIN  fees_per_transaction_t ft ON sa.id = ft.servicesAvailedID INNER JOIN  services_t s ON sa.serviceID = s.serviceID INNER JOIN  sales_invoice_t si ON si.sqNoChar = sq.sqNoChar";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
    
        private void DisplayReportSalesDay()
        {
          
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");

            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesItemTable", GetItemDay()));
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesServiceTable", GetSerDay()));
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

            cmd.CommandText = "SELECT        i.itemName, SUM((IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0) + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) - (IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0)  + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) *(IFNULL(si.sc_pwd_discount, 0) / 100)) AS TOTAL_ITEM FROM sales_quote_t sq INNER join items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN  item_t i ON ia.itemID = i.ID INNER JOIN markup_hist_t mh ON i.ID = mh.itemID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar WHERE(DATE_FORMAT(si.dateOfIssue, '%Y-%m-%d') = CURDATE()) GROUP BY i.itemName  ";

            DatasetReportSales.SalesItemDataTable dSItem = new DatasetReportSales.SalesItemDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private DataTable GetSerDay()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceName, sa.totalCost + IFNULL(ft.feeValue, 0) AS TOTAL_SERVICE FROM sales_quote_t sq INNER JOIN   services_availed_t sa ON sq.sqNoChar = sa.sqNoChar INNER JOIN  fees_per_transaction_t ft ON sa.id = ft.servicesAvailedID INNER JOIN  services_t s ON sa.serviceID = s.serviceID INNER JOIN  sales_invoice_t si ON si.sqNoChar = sq.sqNoChar WHERE(DATE_FORMAT(si.dateOfIssue, '%Y-%m-%d') = CURDATE()) ";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesWeek()
        {
       

            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.SalesReport.rdlc");

            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesItemTable", GetItemWeek()));
            ReportSales.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("SalesServiceTable", GetSerWeek()));
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

            cmd.CommandText = "SELECT        i.itemName, SUM((IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0) + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) - (IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0)  + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) *(IFNULL(si.sc_pwd_discount, 0) / 100)) AS TOTAL_ITEM FROM sales_quote_t sq INNER join items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN  item_t i ON ia.itemID = i.ID INNER JOIN markup_hist_t mh ON i.ID = mh.itemID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar WHERE(WEEK(si.dateOfIssue) = '" + DatePickerItemWeek.SelectedDate.Value.Date.ToShortDateString() + "')  GROUP BY i.itemName  ";

            DatasetReportSales.SalesItemDataTable dSItem = new DatasetReportSales.SalesItemDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private DataTable GetSerWeek()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT        s.serviceName, sa.totalCost + IFNULL(ft.feeValue, 0) AS TOTAL_SERVICE FROM sales_quote_t sq INNER JOIN   services_availed_t sa ON sq.sqNoChar = sa.sqNoChar INNER JOIN  fees_per_transaction_t ft ON sa.id = ft.servicesAvailedID INNER JOIN  services_t s ON sa.serviceID = s.serviceID INNER JOIN  sales_invoice_t si ON si.sqNoChar = sq.sqNoChar WHERE(WEEK(si.dateOfIssue) = '" + DatePickerItemWeek.SelectedDate.Value.Date.ToShortDateString() + "') ";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }

        private void DisplayReportSalesMonth()
        {
           

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

            cmd.CommandText = "SELECT        i.itemName, SUM((IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0) + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) - (IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0)  + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) *(IFNULL(si.sc_pwd_discount, 0) / 100)) AS TOTAL_ITEM FROM sales_quote_t sq INNER join items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN  item_t i ON ia.itemID = i.ID INNER JOIN markup_hist_t mh ON i.ID = mh.itemID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar WHERE(MONTHNAME(si.dateOfIssue) = '" + ((ComboBoxItem)ComboBoxItemMonth.SelectedItem).Content.ToString() + "') GROUP BY i.itemName  ";
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

            cmd.CommandText = "SELECT        s.serviceName, sa.totalCost + IFNULL(ft.feeValue, 0) AS TOTAL_SERVICE FROM sales_quote_t sq INNER JOIN   services_availed_t sa ON sq.sqNoChar = sa.sqNoChar INNER JOIN  fees_per_transaction_t ft ON sa.id = ft.servicesAvailedID INNER JOIN  services_t s ON sa.serviceID = s.serviceID INNER JOIN  sales_invoice_t si ON si.sqNoChar = sq.sqNoChar WHERE(MONTHNAME(si.dateOfIssue) = '" + ((ComboBoxItem)ComboBoxItemMonth.SelectedItem).Content.ToString() + "')  ";
            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesYear()
        {
       

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

            cmd.CommandText = "SELECT        i.itemName, SUM((IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0) + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) - (IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0)  + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) *(IFNULL(si.sc_pwd_discount, 0) / 100)) AS TOTAL_ITEM FROM sales_quote_t sq INNER join items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN  item_t i ON ia.itemID = i.ID INNER JOIN markup_hist_t mh ON i.ID = mh.itemID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar WHERE(YEAR(si.dateOfIssue) = '" + ((ComboBoxItem)ComboBoxItemYear.SelectedItem).Content.ToString() + "')  GROUP BY i.itemName  ";

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

            cmd.CommandText = "SELECT        s.serviceName, sa.totalCost + IFNULL(ft.feeValue, 0) AS TOTAL_SERVICE FROM sales_quote_t sq INNER JOIN   services_availed_t sa ON sq.sqNoChar = sa.sqNoChar INNER JOIN  fees_per_transaction_t ft ON sa.id = ft.servicesAvailedID INNER JOIN  services_t s ON sa.serviceID = s.serviceID INNER JOIN  sales_invoice_t si ON si.sqNoChar = sq.sqNoChar WHERE(YEAR(si.dateOfIssue) = '" + ((ComboBoxItem)ComboBoxItemYear.SelectedItem).Content.ToString() + "')  ";

            DatasetReportSales.services_tDataTable dSItem = new DatasetReportSales.services_tDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSItem);

            return dSItem;

        }
        private void DisplayReportSalesRange()
        {
       
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

            cmd.CommandText = "SELECT        i.itemName, SUM((IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0) + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) - (IFNULL(si.withholdingTax, 0) + IFNULL(si.vat, 0)  + ia.itemQnty * (ia.unitPrice + ia.unitPrice * (mh.markupPerc / 100))) *(IFNULL(si.sc_pwd_discount, 0) / 100)) AS TOTAL_ITEM FROM sales_quote_t sq INNER join items_availed_t ia ON sq.sqNoChar = ia.sqNoChar INNER JOIN  item_t i ON ia.itemID = i.ID INNER JOIN markup_hist_t mh ON i.ID = mh.itemID INNER JOIN  sales_invoice_t si ON sq.sqNoChar = si.sqNoChar WHERE(si.dateOfIssue BETWEEN '" + DatePickerItemStart.Text  + "' AND '" + DatePickerItemEnd.SelectedDate.Value.Date.ToShortDateString() + "') GROUP BY i.itemName  ";

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

            cmd.CommandText = "SELECT        s.serviceName, sa.totalCost + IFNULL(ft.feeValue, 0) AS TOTAL_SERVICE FROM sales_quote_t sq INNER JOIN   services_availed_t sa ON sq.sqNoChar = sa.sqNoChar INNER JOIN  fees_per_transaction_t ft ON sa.id = ft.servicesAvailedID INNER JOIN  services_t s ON sa.serviceID = s.serviceID INNER JOIN  sales_invoice_t si ON si.sqNoChar = sq.sqNoChar WHERE(si.dateOfIssue BETWEEN  '" + DatePickerItemStart.SelectedDate.Value.Date.ToShortDateString() + "' AND '" + DatePickerItemEnd.SelectedDate.Value.Date.ToShortDateString() + "')  ";

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
                ComboBoxItemMonth.Visibility = Visibility.Hidden;
                MonthItem.Visibility = Visibility.Hidden;
                YearItem.Visibility = Visibility.Hidden;
                DatePickerItemStart.Visibility = Visibility.Hidden;
                DatePickerItemEnd.Visibility = Visibility.Hidden;
                ItemStart.Visibility = Visibility.Hidden;
                ItemEnd.Visibility = Visibility.Hidden;
                DatePickerItemWeek.Visibility = Visibility.Hidden;


            }
            if (ComboBoxItemFilter.SelectedIndex == 1)
            {
                ReportSales.Reset();
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
            if (ComboBoxItemFilter.SelectedIndex == 2)
            {
                ReportSales.Reset();
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
            if (ComboBoxItemFilter.SelectedIndex == 3)
            {
                ReportSales.Reset();
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
            if (ComboBoxItemFilter.SelectedIndex == 4)
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
