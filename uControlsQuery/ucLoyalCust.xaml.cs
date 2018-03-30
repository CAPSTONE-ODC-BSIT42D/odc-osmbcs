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
        private void ucLoyalCust_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
            var dbCon = DBConnection.Instance();

            if (this.IsVisible)
            {
                if (dbCon.IsConnect())
                {
                    MainVM.Phases.Clear();
                    string query = "	SELECT cs.companyName, COUNT(si.invoiceNo) as _count FROM cust_supp_t cs JOIN sales_invoice_t si ON cs.companyID = si.custID WHERE companyType = 0 GROUP BY si.custID ORDER BY _count DESC LIMIT 0,10";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    cust_supp_tDataGrid.ItemsSource = fromDbTable.DefaultView;
                    dbCon.Close();
                }
            }
        }
    }
}
