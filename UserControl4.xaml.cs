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
    /// Interaction logic for UserControl4.xaml
    /// </summary>
    public partial class UserControl4 : UserControl
    {
        public UserControl4()
        {
            InitializeComponent();
           
        }
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        
        private void DisplayReport()
        {
            Contract.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", GetContract()));
            Contract.LocalReport.ReportEmbeddedResource = "prototype2.Report6.rdlc";
            Contract.RefreshReport();
        }

        private DataTable GetContract()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "Select e.empfname, e.empmi,e.empLName, e.empAddress, e.empCity, e.empdatefrom, e.empDateTo, j.jobname from emp_cont_t e JOIN job_title_t j ON e.jobID = j.jobID where isDeleted = 0 and emptype = 1 and e.empID  = '"+ MainVM.SelectedEmployeeContractor.EmpID + "'";

            DataSet1.DataTable4DataTable dSContract = new DataSet1.DataTable4DataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSContract);

            return dSContract;

        }

    

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Contract.RefreshReport();
        }

   

        private void btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            DisplayReport();
           
        }

        private void btnback_Click(object sender, RoutedEventArgs e)
        {
            UcContract.Visibility = Visibility.Hidden;
            
        }
    }
}
