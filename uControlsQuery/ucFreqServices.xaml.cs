using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

using System.Configuration;
using System.Windows;
namespace prototype2
{
    /// <summary>
    /// Interaction logic for FreqServices.xaml
    /// </summary>
    public partial class FreqServices : UserControl
    {
        #region MySqlConnection Connection
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public FreqServices()
        {
            InitializeComponent();
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
                MySqlCommand cmd = new MySqlCommand("select ServiceName, count(sa.serviceid) as NoTimesSer from services_t s, services_availed_t sa where s.serviceid = sa.serviceid group by serviceName", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                FreqServicesDataGrid.DataContext = ds;
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
