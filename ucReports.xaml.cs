using Microsoft.Reporting.WebForms;
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

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucReports.xaml
    /// </summary>
    public partial class ucReports : UserControl
    {
        public ucReports()
        {
            InitializeComponent();
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Object SELECTEDINDEX = ComboBoxReport.SelectedIndex;
            if (SELECTEDINDEX.Equals(0))
            {
                UCReportService.Visibility = Visibility.Visible;
                UCReportsItem.Visibility = Visibility.Hidden;
                UCReportSales.Visibility = Visibility.Hidden;



            }
            if (SELECTEDINDEX.Equals(1))
            {
                UCReportsItem.Visibility = Visibility.Visible;
                UCReportService.Visibility = Visibility.Hidden;
                UCReportSales.Visibility = Visibility.Hidden;
            }
            if (SELECTEDINDEX.Equals(2))
            {

                UCReportsItem.Visibility = Visibility.Hidden;
                UCReportService.Visibility = Visibility.Hidden;
                UCReportSales.Visibility = Visibility.Visible;
            }
        }
    }
}
