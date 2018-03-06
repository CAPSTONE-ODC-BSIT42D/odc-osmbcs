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
        #region MySqlConnection Connection
        MySqlConnection conn = new
            MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public ucFreqContractor()
        {
            InitializeComponent();
            binddatagrid();
        }
        private void binddatagrid()
        {
            //var dbCon = DBConnection.Instance();
            //dbCon.IsConnect();
            //MySqlCommand cmd = new MySqlCommand();
            //cmd.Connection = dbCon.Connection;
            //cmd.CommandType = CommandType.Text;

            //cmd.CommandText = "select itemname, itemDescr, count(itemName) from item_t  group by itemName,itemDescr";



            //odc_dbDataSet.item_tDataTable dSItem = new odc_dbDataSet.item_tDataTable();

            //MySqlDataAdapter mySqlDa = new MySqlDataAdapter(cmd);
            //mySqlDa.Fill(dSItem);
            //FreqItemsDataGrid.ItemsSource = dSItem.DefaultView;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select Concat(e.empFname + '  '+ e.empLname ) as EmpName, j.jobname, count(ae.empid) as NoTimesEmp, ServiceName from emp_cont_t  e, Job_Title_t j, assigned_employees_t ae, service_sched_t ss, services_availed_t sa, services_t s where s.serviceid =sa.serviceid and ss.serviceAvailedid= sa.id and ss.serviceschedid=ae.serviceschedid and ae.empid = e.empid and j.jobId = e.jobid group by j.jobname, serviceName", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                FreqContractorDataGrid.DataContext = ds;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                conn.Close();
            }


            #endregion
        }
    }
}
