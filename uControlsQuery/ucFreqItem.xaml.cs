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
using System.Collections.ObjectModel;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucFreqItem.xaml
    /// </summary>
    public partial class ucFreqItem : UserControl
    {
        MySqlConnection conn = new
MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        public ucFreqItem()
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
                    string query = "	SELECT i.itemName, COUNT(ia.id) _count FROM item_t i JOIN items_availed_t ia ON i.id = ia.itemID JOIN sales_quote_t sq ON sq.sqNoChar = ia.sqNoChar    JOIN sales_invoice_t si ON si.sqNoChar = sq.sqNoChar GROUP BY ia.itemID ORDER BY _count DESC LIMIT 0, 20";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    item_tDataGrid.ItemsSource = fromDbTable.DefaultView;
                    dbCon.Close();
                }
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
        
    }
}
