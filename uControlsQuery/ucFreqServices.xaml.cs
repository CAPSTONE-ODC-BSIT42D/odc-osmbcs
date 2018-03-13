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
        
        MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["prototype2.Properties.Settings.odc_dbConnectionString"].ConnectionString);
        public FreqServices()
        {
            InitializeComponent();
        }
        private void binddatagrid()
        {
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
