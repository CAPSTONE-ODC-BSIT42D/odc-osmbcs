using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

using System.Configuration;
using System.Windows;


namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucFreqContractor.xaml
    /// </summary>
    public partial class ucFreqContractor : UserControl
    {
        public ucFreqContractor()
        {
            InitializeComponent();
        }
        

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
            var dbCon = DBConnection.Instance();

            if (this.IsVisible)
            {
                if (dbCon.IsConnect())
                {
                    MainVM.Phases.Clear();
                    string query = "SELECT CONCAT(e.empFName,' ',e.empLName) as _fullName, j.jobName, COUNT(ae.assignEmployeeID) AS _count FROM assigned_employees_t ae JOIN emp_cont_t e ON e.empID = ae.empID JOIN job_title_t j ON j.jobID = e.jobID JOIN service_sched_t ss ON ss.serviceSchedID = ae.serviceSchedID GROUP BY ae.empID ORDER BY _count DESC LIMIT 0, 10";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    freqCont.ItemsSource = fromDbTable.DefaultView;
                    dbCon.Close();
                }
            }
        }
    }
}
