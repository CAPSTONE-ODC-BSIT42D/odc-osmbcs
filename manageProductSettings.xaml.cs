using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for manageProductSettings.xaml
    /// </summary>
    public partial class manageProductSettings : Window
    {
        public manageProductSettings()
        {
            InitializeComponent();
        }
        private static String dbname = "odc_db";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setWindowControls();
        }

        private void setWindowControls()
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM ITEM_CATEGORY_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                invProductsCategoryLb.ItemsSource = fromDb.Tables["t"].DefaultView;
                invProductsCategoryLb.DisplayMemberPath = "categoryName";
                invProductsCategoryLb.SelectedValuePath = "categoryID";
            }
        }

        private void deleteCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            MessageBox.Show(""+ invProductsCategoryLb.SelectedValue);
            if (dbCon.IsConnect())
            {
                string query = "DELETE FROM `odc_db`.`item_category_t` WHERE `categoryID`='" + invProductsCategoryLb.SelectedValue + "';";
                if (dbCon.deleteQuery(query, dbCon.Connection))
                {
                    dbCon.Close();
                    setWindowControls();
                    MessageBox.Show("Deleted the category");
                }


            }

        }

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                string query = "INSERT INTO `odc_db`.`item_category_t` (`categoryName`) VALUES('" + invCategoryTb.Text + "')";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    MessageBox.Show("Added");
                    setWindowControls();
                    dbCon.Close();
                }


            }
        }
    }
}
