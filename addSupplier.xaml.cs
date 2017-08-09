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
    /// Interaction logic for addSupplier.xaml
    /// </summary>
    public partial class addSupplier : Window
    {
        public String Name { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Number { get; set; }
        public String Email { get; set; }
        public object locProvinceId { get; set; }
        public object cityID { get; set; }
        public addSupplier()
        {
            InitializeComponent();
            custCompanyNameTb.DataContext = this;
            locationAddressTb.DataContext = this;
            cityCb.DataContext = this;
            emailAddress.DataContext = this;
            officeNumber.DataContext = this;
            custProvinceCust.DataContext = this;

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
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            MessageBoxResult result = MessageBox.Show("Do you want to save this new supplier?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO location_details_t (locationAddress,locationCityID,locationProvinceID) VALUES ('" + locationAddressTb.Text + "','" + cityCb.SelectedValue + "', '" + custProvinceCust.SelectedValue + "')";

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "select last_insert_id() from location_details_t";
                        MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                        DataSet fromDb = new DataSet();
                        dataAdapter.Fill(fromDb, "t");
                        string locID = "";
                        foreach (DataRow myRow in fromDb.Tables[0].Rows)
                        {
                            locID = myRow[0].ToString();
                        }
                        query = "INSERT INTO supplier_t (suppCompanyName,suppAddInfo,locationID) VALUES ('" + custCompanyNameTb.Text + "','" + custAddInfoTb.Text + "','" + locID + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            query = "select last_insert_id() from supplier_t";
                            dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                            fromDb = new DataSet();
                            dataAdapter.Fill(fromDb, "t");
                            string custId = "";
                            foreach (DataRow myRow in fromDb.Tables[0].Rows)
                            {
                                custId = myRow[0].ToString();
                            }
                            query = "INSERT INTO supplier_contacts_t (suppID,sOfficeNo,sEmailAddress) VALUES ('" + custId + "','" + officeNumber.Text + "','" + emailAddress.Text + "')";
                            if (dbCon.insertQuery(query, dbCon.Connection))
                            {
                                MessageBox.Show("Saved");
                                //clearing textboxes
                                locationAddressTb.Clear();
                                cityCb.SelectedItem = -1;
                                custProvinceCust.SelectedItem = -1;
                                custCompanyNameTb.Clear();
                                custAddInfoTb.Clear();
                                officeNumber.Clear();
                                emailAddress.Clear();
                                this.Close();
                            }
                        }
                    }
                }
            }
            else if (result == MessageBoxResult.No)
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
            if (custCompanyNameTb.Text.Equals("")||locationAddressTb.Text.Equals("")||cityCb.SelectedIndex==-1||custProvinceCust.SelectedIndex==-1||officeNumber.Text.Equals("")||emailAddress.Text.Equals(""))
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
