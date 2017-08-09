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
    /// Interaction logic for editCustomer.xaml
    /// </summary>
    public partial class editCustomer: Window
    {
        public String id;
        public String locId;
        public String CompName { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Number { get; set; }
        public String Email { get; set; }
        public object locProvinceId { get; set; }
        public object cityID { get; set; }
        public editCustomer(String id)
        {
            InitializeComponent();
            this.id = id;
            custCompanyNameTb.DataContext = this;
            locationAddressTb.DataContext = this;
            
            emailAddress.DataContext = this;
            officeNumber.DataContext = this;
            custProvinceCust.DataContext = this;
            cityCb.DataContext = this;
            setControlValuesSynced(id);
        }
        private static String dbname = "odc_db";
        private void setControlsValues()
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custProvinceCust.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
        }
        private void setControlValuesSynced(String id)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                string query ="SELECT c.custID, c.custCompanyName, c.custAddInfo,c.locationId, cc.officePhoneNo, cc.emailAddress,ld.locationAddress,ld.locationCityID,lp.locProvinceID FROM customer_t c " +
                    "JOIN customer_contacts_t cc ON c.custID = cc.custID " +
                    "JOIN location_details_t ld ON c.locationID = ld.locationID " +
                    "JOIN provinces_t lp ON ld.locationProvinceID = lp.locProvinceID WHERE c.custID = '"+ id + "';";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet(); 
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    int locProvId = Int32.Parse(dr["locProvinceID"].ToString());
                    custProvinceCust.SelectedIndex = locProvId - 1;
                    locId = dr["locationId"].ToString();
                    CompName = dr["custCompanyName"].ToString();
                    custAddInfoTb.Text = dr["custAddInfo"].ToString();
                    
                    int locCityId = Int32.Parse(dr["locationCityID"].ToString());
                    cityCb.SelectedValue = locCityId;
                    Address = dr["locationAddress"].ToString();
                    Number = dr["officePhoneNo"].ToString();
                    Email = dr["emailAddress"].ToString();
                }
                dbCon.Close();
            }
        }
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result==MessageBoxResult.Yes)
            {
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `customer_t` SET `custCompanyname`='" + custCompanyNameTb.Text + "',`custAddInfo`='" + custAddInfoTb.Text + "' WHERE `custID`='" + id + "';";

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "UPDATE `location_details_t` SET `locationAddress`='" + locationAddressTb.Text + "',`locationCityID`='" + cityCb.SelectedValue + "',`locationProvinceID`='" + custProvinceCust.SelectedValue + "' WHERE `locationId`='" + locId + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {

                            query = "UPDATE `customer_contacts_t` SET `officePhoneNo`='" + officeNumber.Text + "',`emailAddress`='" + emailAddress.Text + "' WHERE `custID`='" + id + "';";
                            if (dbCon.insertQuery(query, dbCon.Connection))
                            {
                                MessageBox.Show("Updated");

                                //clearing textboxes
                                custAddInfoTb.Clear();
                                locationAddressTb.Clear();
                                cityCb.SelectedValue = -1;
                                custProvinceCust.SelectedValue = -1;
                                officeNumber.Clear();
                                emailAddress.Clear();
                                this.Close();
                            }
                        }
                    }
                }
            }
            else if(result == MessageBoxResult.No)
            {
                this.Close();
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setControlsValues();

        }

        private void custCompanyNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(custCompanyNameTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void locationAddressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(locationAddressTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void locationCityTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(cityCb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void custProvinceCust_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect() && custProvinceCust.SelectedIndex != -1)
            {
                string query = "SELECT * FROM city_by_province_t cp WHERE provinceID = '" + custProvinceCust.SelectedValue + "'";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                cityCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (System.Windows.Controls.Validation.GetHasError(custProvinceCust) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void officeNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(officeNumber) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void emailAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(emailAddress) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }
        private void validateTextBoxes()
        {
            if (custCompanyNameTb.Text.Equals("") || locationAddressTb.Text.Equals("") || cityCb.SelectedIndex==-1 || custProvinceCust.SelectedIndex == -1 || officeNumber.Text.Equals("") || emailAddress.Text.Equals(""))
            {
                saveBtn.IsEnabled = false;
            }
            else
            {
                saveBtn.IsEnabled = true;
            }
        }

        private void cityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(cityCb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }
    }
}
