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
            this.DataContext = MainMenu.MainVM;
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
                MessageBox.Show("Employee position already exists");
            }
            else
            {
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO `odc_db`.`position_t` (`positionName`) VALUES('" + empPosNewTb.Text + "')";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Employee Poisition successfully added");
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
                    string query = "UPDATE `odc_db`.`position_t` set `positionName` = '" + empPosNewTb.Text + "' where positionID = '" + MainMenu.MainVM.SelectedEmpPosition.PositionID + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Employee Poisition saved");
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
                        string query = "DELETE FROM `odc_db`.`position_t` WHERE `positionID`='" + MainMenu.MainVM.SelectedEmpPosition.PositionID + "';";

                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            dbCon.Close();
                            MessageBox.Show("Employee position successfully deleted.");
                            setListBoxControls();
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            else
            {
                MessageBox.Show("Select an employee position first.");
            }
            
        }

        private void editEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;

            if (employeePositionLb.SelectedItems.Count > 0)
            {

                empPosNewTb.Text = MainMenu.MainVM.SelectedEmpPosition.PositionName;
            }
            else
            {
                MessageBox.Show("Please select an employee position first.");
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
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                MainMenu.MainVM.EmpPosition.Clear();
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainMenu.MainVM.EmpPosition.Add(new EmpPosition(){ PositionID = dr["positionid"].ToString(), PositionName = dr["positionName"].ToString() });
                }
                dbCon.Close();
            }
        }
    }
}
