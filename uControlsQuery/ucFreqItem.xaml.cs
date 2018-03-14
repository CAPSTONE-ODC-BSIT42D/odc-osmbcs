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


namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucFreqItem.xaml
    /// </summary>
    public partial class ucFreqItem : UserControl
    {
        MySqlConnection conn = new
MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public ucFreqItem()
        {
            InitializeComponent();
          
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
        private void binddatagrid()
        {



            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT        i.itemName, COUNT(ia.id) AS NoOfTimes FROM            item_t i INNER JOIN  items_availed_t ia ON i.ID = ia.itemID INNER JOIN  sales_quote_t sq ON sq.sqNoChar = ia.sqNoChar WHERE(sq.status = 'ACCEPTED') GROUP BY ia.itemID ORDER BY NoOfTimes DESC", conn);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
               QueriesDataSet ds = new QueriesDataSet();
                adp.Fill(ds);
                item_tDataGrid.DataContext = ds;
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
    }
}
