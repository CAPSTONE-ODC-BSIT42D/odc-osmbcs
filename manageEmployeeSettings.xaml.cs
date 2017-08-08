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
    /// Interaction logic for manageEmployeeSettings.xaml
    /// </summary>
    public partial class manageEmployeeSettings : Window
    {
        public manageEmployeeSettings()
        {
            InitializeComponent();
        }
        private static String dbname = "odc_db";

        //EMPLOYEE PART
        string positionID = "";
        private void addEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (employeePositionLb.Items.Contains(empPosNewTb.Text))
            {
                MessageBox.Show("Already in the list.");
            }
            else
            {
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO `odc_db`.`position_t` (`positionName`) VALUES('" + empPosNewTb.Text + "')";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Added");
                        empPosNewTb.Clear();
                        setListBoxControls();
                        dbCon.Close();
                    }
                }
            }
            
        }

        private void saveEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (String.IsNullOrWhiteSpace(empPosNewTb.Text))
            {
                MessageBox.Show("Employee Position must be filled");
            }
            else
            {
                if (employeePositionLb.Items.Contains(empPosNewTb.Text))
                {
                    MessageBox.Show("Already in the list.");
                }
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `odc_db`.`position_t` set `positionName` = '" + empPosNewTb.Text + "' where positionID = '" + positionID + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Saved");
                        empPosNewTb.Clear();
                        setListBoxControls();
                        dbCon.Close();
                    }
                }
            }
            
        }
        private void deleteEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            if (employeePositionLb.SelectedItems.Count > 0)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (dbCon.IsConnect())
                {
                    try
                    {
                        foreach (object list in employeePositionLb.Items)
                        {
                            if (employeePositionLb.SelectedItems.Count != 0)
                            {
                                positionID = (String)employeePositionLb.SelectedValue.ToString();
                                empPosNewTb.Text = (employeePositionLb.SelectedItem as DataRowView)["positionName"].ToString();
                                string query = "DELETE FROM `odc_db`.`position_t` WHERE `positionID`='" + employeePositionLb.SelectedValue + "';";

                                if (dbCon.insertQuery(query, dbCon.Connection))
                                {
                                    dbCon.Close();
                                    MessageBox.Show("Position deleted.");
                                    setListBoxControls();
                                }
                            }
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            else
            {
                MessageBox.Show("Select a position first.");
            }
            
        }

        private void editEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;

            if (employeePositionLb.SelectedItems.Count > 0)
            {
                positionID = employeePositionLb.SelectedValue.ToString();
                empPosNewTb.Text = (employeePositionLb.SelectedItem as DataRowView)["positionName"].ToString();
                saveEmpPosBtn.Visibility = Visibility.Visible;

                string query = "SELECT * FROM position_t WHERE positionid = '" + positionID + "';";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);

                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                DataTable fromDbTable = new DataTable();
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    try {
                        empPosNewTb.Text = dr["empPosNewTb"].ToString();

                    }//try                        
                    catch (Exception) { throw;   }
                }
            }
            else
            {
                MessageBox.Show("Please select a position first.");
            }
            dbCon.Close();
        }

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setListBoxControls();
        }

        private void setListBoxControls()
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM POSITION_T;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t1");
                employeePositionLb.ItemsSource = fromDb.Tables["t1"].DefaultView;
                employeePositionLb.DisplayMemberPath = "positionName";
                employeePositionLb.SelectedValuePath = "positionId";
                dbCon.Close();
            }
        }
    }
}
