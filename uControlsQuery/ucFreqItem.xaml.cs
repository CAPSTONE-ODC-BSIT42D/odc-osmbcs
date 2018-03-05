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
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucFreqItem.xaml
    /// </summary>
    public partial class ucFreqItem : UserControl
    {
        public ucFreqItem()
        {
            InitializeComponent();
            binddatagrid();
        }

        private void binddatagrid()
        {
            var dbCon = DBConnection.Instance();
            dbCon.IsConnect();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbCon.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "select itemname, itemDescr, count(itemName) from item_t  group by itemName";

         

          
        }

        private void comboBoxFreqItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
