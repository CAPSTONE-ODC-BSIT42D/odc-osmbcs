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
    public partial class ucNoticeOfEmployment : UserControl
    {
        public ucNoticeOfEmployment()
        {
            InitializeComponent();
            
        }
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private void DisplayReport()
        {
            ucReportViewer.Reset();
            var rNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("prototype2.RDLC.NoticeOfEmployment.rdlc");
            ucReportViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("DataSet1", GetContract()));
            ucReportViewer.LoadReport(rNames);
            ucReportViewer.ProcessingMode = Syncfusion.Windows.Reports.Viewer.ProcessingMode.Local;
            ucReportViewer.RefreshReport();
        }

        private DataTable GetContract()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "Select e.empfname, e.empmi,e.empLName, e.empAddress, e.empCity, e.empdatefrom, e.empDateTo, j.jobname from emp_cont_t e JOIN job_title_t j ON e.jobID = j.jobID where isDeleted = 0 and emptype = 1 and e.empID  = '"+ MainVM.SelectedEmployeeContractor.EmpID + "'";

            DataSet1.noticeOfEmploymentDataTable dSContract = new DataSet1.noticeOfEmploymentDataTable();

            MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            mySqlDa.Fill(dSContract);

            return dSContract;

        }

    

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsLoaded)
            {
                GetContract();
            }
            ucReportViewer.RefreshReport();
        }
        
    }
}
