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
        MySqlConnection conn = new
MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public ucFreqContractor()
        {
            InitializeComponent();
            //binddatagrid();
        }
        private void binddatagrid()
        {

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT        e.empFName + e.empLName AS NAME, j.jobName AS Position, COUNT(ae.assignEmployeeID) AS NoOfTimes FROM            assigned_employees_t ae INNER JOIN  emp_cont_t e ON e.empID = ae.empID INNER JOIN  job_title_t j ON j.jobID = e.jobID INNER JOIN   service_sched_t ss ON ss.serviceSchedID = ae.serviceSchedID GROUP BY ae.empID, ae.serviceSchedID ORDER BY NoOfTimes DESC", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                QueriesDataSet ds = new QueriesDataSet();
                adp.Fill(ds);
                job_title_tDataGrid.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Do not load your data at design time.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Load your data here and assign the result to the CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                binddatagrid();

            }
            else
            {

            }
        }
    }
}
