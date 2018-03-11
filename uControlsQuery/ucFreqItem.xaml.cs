using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

using System.Configuration;
using System.Windows;

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
           
        }
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            binddatagrid();
        }
        private void binddatagrid()
        {

            string MyConString ="SERVER=myserver;" + "DATABASE=myDb;" + "UID=username;" +  "PASSWORD=pass;Convert Zero Datetime=True";

            string sql = "select * from jos_categories";

            var connection = new MySqlConnection(MyConString);
            var cmdSel = new MySqlCommand(sql, connection);
            var dt = new DataTable();
            var da = new MySqlDataAdapter(cmdSel);
            da.Fill(dt);
            dataGrid1.DataContext = dt;





        }

    private void comboBoxFreqItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
