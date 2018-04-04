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
    /// Interaction logic for ucMostSupplier.xaml
    /// </summary>
    public partial class ucMostSupplier : UserControl
    {

        MySqlConnection conn = new
MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public ucMostSupplier()
        {
            InitializeComponent();
            //binddatagrid();
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
                    string query = "	SELECT cs.companyName, COUNT(po.poNumChar) as _count FROM cust_supp_t cs JOIN purchase_order_t po ON po.suppID = cs.companyID GROUP BY po.suppID   ORDER BY _count DESC LIMIT 0, 20; ";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    supplierDataGrid.ItemsSource = fromDbTable.DefaultView;
                    dbCon.Close();
                }
            }
        }
    }
}
