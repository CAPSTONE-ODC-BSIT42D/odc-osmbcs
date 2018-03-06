using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

using System.Configuration;
using System.Windows;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        #region MySqlConnection Connection
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public UserControl1()
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
                MySqlCommand cmd = new MySqlCommand("select c.companyName, count(sp.invoiceno) as NoTimesCust from cust_supp_t c, sales_invoice_t s, si_payment_t sp where c.companyid = s.custid and s.invoiceno = sp.invoiceno group by c.companyName", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds, "LoadDataBinding");
                LoyalCustDataGrid.DataContext = ds;
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
