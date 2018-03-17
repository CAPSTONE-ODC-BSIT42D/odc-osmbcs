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
          

            //DisplayReport();
        }
        private void DisplayReport()
        {
            ReportPurchase.Reset();

            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
 
          
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("PurchaseTable", GetPurchase()));
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
          
          
            cmd.CommandText = " SELECT        itemName, SUM(Total) AS Expr1 FROM po_report_view GROUP BY itemName   ";

            DataSetReportPurchase.PurchaseTableDataTable dSPurchase = new DataSetReportPurchase.PurchaseTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }

      
        private void DisplayReportPurchaseYear()
        {
   
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("PurchaseTable", GetPurchaseYear()));
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
            string year = ((ComboBoxItem)ComboBoxYearSales.SelectedItem).Content.ToString();//same problem
        
            cmd.CommandText = "   SELECT        itemName, SUM(Total) AS Expr1 FROM po_report_view  WHERE(YEAR(_date) = '" + year + "')  GROUP BY itemName  ";

            DataSetReportPurchase.PurchaseTableDataTable dSPurchase = new DataSetReportPurchase.PurchaseTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void DisplayReportPurchaseMonth()
        {

            ReportPurchase.Reset();

            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("PurchaseTable", GetPurchaseMonth()));
            ReportPurchase.LoadReport(rNames);
            ReportPurchase.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ReportPurchase.RefreshReport();

        
        }

        private DataTable GetPurchaseMonth()
        {
            string month = ((ComboBoxItem)ComboBoxMonthSales.SelectedItem).Content.ToString();//same problem
    
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;
       
      
            cmd.CommandText = "     SELECT        itemName, SUM(Total) AS Expr1 FROM po_report_view WHERE (MONTHNAME(_date) = '" + month + "') AND (YEAR(_date) = YEAR(CURDATE())) GROUP BY itemName ";

            DataSetReportPurchase.PurchaseTableDataTable dSPurchase = new DataSetReportPurchase.PurchaseTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void DisplayReportPurchaseRange()
        {
    
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.rdlcfiles.PurchaseReport.rdlc");
            ReportPurchase.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("PurchaseTable", GetPurchaseRange()));
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

            cmd.CommandText = "  SELECT        itemName, SUM(Total) AS Expr1 FROM po_report_view WHERE(_date BETWEEN '" + SalesDateStart.SelectedDate.Value.ToString("yyyy-MM-dd") + "' AND '"+ SalesDateEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + "')  GROUP BY itemName  ";

            DataSetReportPurchase.PurchaseTableDataTable dSPurchase = new DataSetReportPurchase.PurchaseTableDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSPurchase);

            return dSPurchase;


        }
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                DisplayReport();

            }
            else
            {
                ReportPurchase.Reset();
            }
        }

        private void ComboBoxSalesFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
            
        {
           
            Object SELECTEDINDEX = ComboBoxSalesFilter.SelectedIndex;
            if (SELECTEDINDEX.Equals(0))
            {
                ReportPurchase.Reset();
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Visible;
                MonthSales.Visibility = Visibility.Visible;
                YearSales.Visibility = Visibility.Hidden;
                SalesDateStart.Visibility = Visibility.Hidden;
                SalesDateEnd.Visibility = Visibility.Hidden;
                labelstart.Visibility = Visibility.Hidden;
                labelend.Visibility = Visibility.Hidden;

                labelweek.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(1))

            {

                ReportPurchase.Reset();
                ComboBoxYearSales.Visibility = Visibility.Visible;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Visible;
                SalesDateStart.Visibility = Visibility.Hidden;
                SalesDateEnd.Visibility = Visibility.Hidden;
                labelstart.Visibility = Visibility.Hidden;
                labelend.Visibility = Visibility.Hidden;

                labelweek.Visibility = Visibility.Hidden;
            }
            if (SELECTEDINDEX.Equals(2))
            {
                ReportPurchase.Reset();
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Hidden;

                SalesDateStart.Visibility = Visibility.Visible;
                SalesDateEnd.Visibility = Visibility.Visible;
                labelstart.Visibility = Visibility.Visible;
                labelend.Visibility = Visibility.Visible;

                labelweek.Visibility = Visibility.Hidden;
            }
            if (SELECTEDINDEX.Equals(3))
            {
            

            }
            if (SELECTEDINDEX.Equals(4))
            {
             

            }
        }

        private void ComboBoxMonthSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
    

        }
        private void ComboBoxYearSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayReportPurchaseYear();
        }
        private void DatePickerStartSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
     
        }
        private void DatePickerEndSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
      
        }

        private void DatePickerWeekSales_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void ComboBoxMonthSales_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            DisplayReportPurchaseMonth();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object SELECTEDINDEX = ComboBoxSalesFilter.SelectedIndex;
       
            if (SELECTEDINDEX.Equals(0))
            {
                ReportPurchase.Reset();
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Visible;
                MonthSales.Visibility = Visibility.Visible;
                YearSales.Visibility = Visibility.Hidden;
                SalesDateStart.Visibility = Visibility.Hidden;
                SalesDateEnd.Visibility = Visibility.Hidden;
                labelstart.Visibility = Visibility.Hidden;
                labelend.Visibility = Visibility.Hidden;

                labelweek.Visibility = Visibility.Hidden;


            }
            if (SELECTEDINDEX.Equals(1))

            {

                ReportPurchase.Reset();
                ComboBoxYearSales.Visibility = Visibility.Visible;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Visible;
                SalesDateStart.Visibility = Visibility.Hidden;
                SalesDateEnd.Visibility = Visibility.Hidden;
                labelstart.Visibility = Visibility.Hidden;
                labelend.Visibility = Visibility.Hidden;

                labelweek.Visibility = Visibility.Hidden;
            }
            if (SELECTEDINDEX.Equals(2))
            {
                ReportPurchase.Reset();
                ComboBoxYearSales.Visibility = Visibility.Hidden;
                ComboBoxMonthSales.Visibility = Visibility.Hidden;
                MonthSales.Visibility = Visibility.Hidden;
                YearSales.Visibility = Visibility.Hidden;

                SalesDateStart.Visibility = Visibility.Visible;
                SalesDateEnd.Visibility = Visibility.Visible;
                labelstart.Visibility = Visibility.Visible;
                labelend.Visibility = Visibility.Visible;

                labelweek.Visibility = Visibility.Hidden;
            }
            if (SELECTEDINDEX.Equals(3))
            {
                
               
            }
            if (SELECTEDINDEX.Equals(4))
            {
            

            }
        }

        private void GoButtonPurchase_Click(object sender, RoutedEventArgs e)
        {
            DisplayReportPurchaseRange();
        }
    }
}
