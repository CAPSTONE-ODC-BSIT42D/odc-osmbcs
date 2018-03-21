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
            MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
            var dbCon = DBConnection.Instance();

            if (this.IsVisible)
            {
                if (dbCon.IsConnect())
                {
                    MainVM.Phases.Clear();
                    string query = "SELECT CONCAT(e.empFName,' ',e.empLName) AS _name, p.positionName, COUNT(ae.assignEmployeeID) AS _count FROM assigned_employees_t ae JOIN emp_cont_t e ON e.empID = e.empID JOIN position_t p ON p.positionID = e.positionID JOIN service_sched_t ss ON ss.serviceSchedID = ae.serviceSchedID GROUP BY ae.empID, ae.serviceSchedID ORDER BY _count DESC LIMIT 0, 10";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    employeeDataGrid.ItemsSource = fromDbTable.DefaultView;
                    dbCon.Close();
                }
            }
        }
    }
}