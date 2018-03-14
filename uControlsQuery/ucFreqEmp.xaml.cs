using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;
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


//Using namespaces 
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

using System.Configuration;
using System.Windows;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucFreqEmp.xaml
    /// </summary>
    public partial class ucFreqEmp : UserControl
    {
        MySqlConnection conn = new
MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public ucFreqEmp()
        {
            InitializeComponent();
            // binddatagrid();
        }

        private void binddatagrid()
        {



            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT        e.empFName + e.empLName AS NAME, p.positionName, COUNT(ae.assignEmployeeID) AS NoOfTimes FROM            assigned_employees_t ae INNER JOIN  emp_cont_t e ON e.empID = ae.empID INNER JOIN  position_t p ON p.positionid = e.positionID INNER JOIN  service_sched_t ss ON ss.serviceSchedID = ae.serviceSchedID GROUP BY ae.empID, ae.serviceSchedID ORDER BY NoOfTimes DESC", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                QueriesDataSet ds = new QueriesDataSet();
                adp.Fill(ds);
                employeeDataGrid.DataContext = ds;
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