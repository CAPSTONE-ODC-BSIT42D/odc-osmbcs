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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ucLoyalcust : UserControl
    {

        MySqlConnection conn = new
MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public ucLoyalcust()
        {
            InitializeComponent();
           // binddatagrid();
        }
        private void binddatagrid()
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT        cs.companyName, COUNT(si.invoiceNo) AS NoOfTransaction FROM            cust_supp_t cs INNER JOIN sales_invoice_t si ON cs.companyID = si.custID GROUP BY si.custID ORDER BY NoOfTransaction DESC", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                QueriesDataSet ds = new QueriesDataSet();
                adp.Fill(ds);
                cust_supp_tDataGrid.DataContext = ds;
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

        private void ucLoyalCust_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
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
