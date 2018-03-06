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
            ucReportViewer.DataSources.Add(new Syncfusion.Windows.Reports.ReportDataSource("noticeOfemploymentDataTable", GetContract()));
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

            cmd.CommandText = "                  SELECT        e.empFName, e.empMI, e.empLName, e.empAddress, e.empDateFrom, e.empDateTo, j.jobName FROM emp_cont_t e INNER JOIN job_title_t j ON e.jobID = j.jobID WHERE(e.isDeleted = 0) AND(e.empType = 1) AND(CONCAT(e.empFName, ' ', e.empMI, ' ', e.empLName) = '"+ MainVM.SelectedEmployeeContractor.EmpID + "'";

            DataSet1.noticeOfemploymentDataTableDataTable dSContract = new DataSet1.noticeOfemploymentDataTableDataTable();

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
