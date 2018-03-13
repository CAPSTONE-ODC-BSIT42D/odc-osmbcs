using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

using System.Configuration;
using System.Windows;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucMostSupplier.xaml
    /// </summary>
    public partial class ucMostSupplier : UserControl
    {
       
        public ucMostSupplier()
        {
            InitializeComponent();
            //binddatagrid();
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
