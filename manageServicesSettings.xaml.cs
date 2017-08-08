
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
    /// Interaction logic for manageServicesSettings.xaml
    /// </summary>
    public partial class manageServicesSettings : Window
    {
        private static String dbname = "odc_db";
        public String Name { get; set; }
        String id = "";
        public manageServicesSettings()
        {
            InitializeComponent();
            serviceName.DataContext = this;
        }

        private void addServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            serviceTypeList.Visibility = Visibility.Collapsed;
            serviceTypeAdd.Visibility = Visibility.Visible;
        }

        private void cancelServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            serviceTypeList.Visibility = Visibility.Visible;
            serviceTypeAdd.Visibility = Visibility.Collapsed;
            setWindowControls();
        }

        private void setWindowControls()
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM service_t;";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                serviceTypeDg.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (dbCon.IsConnect())
            {
                string query = "SELECT * FROM provinces_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                custProvinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
        }

        private void serviceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(serviceName) == true)
                saveServiceTypeBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void servicePrice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            validateTextBoxes();
        }

        private void validateTextBoxes()
        {
            if (serviceName.Text.Equals("") || servicePrice.Value==0)
            {
                saveServiceTypeBtn.IsEnabled = false;
            }
            else
            {
                saveServiceTypeBtn.IsEnabled = true;
            }
        }

        private void saveServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            MessageBoxResult result = MessageBox.Show("Do you want to save this service type?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (id.Equals(""))
                {
                    string query = "INSERT INTO service_t (serviceName,serviceDesc,servicePrice) VALUES ('" + serviceName.Text + "','" + serviceDesc.Text + "', '" + servicePrice.Value + "')";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Service type added!");
                        serviceTypeList.Visibility = Visibility.Visible;
                        serviceTypeAdd.Visibility = Visibility.Collapsed;

                        //clearing textboxes
                        serviceName.Clear();
                        serviceDesc.Clear();
                        servicePrice.Value = 0;
                        setWindowControls();
                    }
                }
                else
                {
                    string query = "UPDATE `service_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + id + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Updated");
                        id = "";
                        serviceTypeList.Visibility = Visibility.Visible;
                        serviceTypeAdd.Visibility = Visibility.Collapsed;
                        setWindowControls();
                    }
                }
                
            }
            else if (result == MessageBoxResult.No)
            {
                for (int x = 1; x < serviceTypeGrid.Children.Count; x++)
                {
                    serviceTypeGrid.Children[x].Visibility = Visibility.Collapsed;
                }
                serviceTypeList.Visibility = Visibility.Visible;
                setWindowControls();
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
            
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (serviceTypeDg.SelectedItems.Count > 0)
            {
                id = (serviceTypeDg.Columns[0].GetCellContent(serviceTypeDg.SelectedItem) as TextBlock).Text;
                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
                string query = "SELECT * FROM service_t where serviceID = '"+id+"';";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                DataTable fromDbTable = new DataTable();
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    try
                    {
                        serviceName.Text = dr["serviceName"].ToString();
                        serviceDesc.Text = dr["serviceDesc"].ToString();
                        servicePrice.Text = dr["servicePrice"].ToString();


                        serviceName.Clear();
                        serviceDesc.Clear();
                        servicePrice.Value = 0;
                    }
                    catch (Exception)
                    {

                        serviceName.Clear();
                        serviceDesc.Clear();
                        servicePrice.Value = 0;
                        throw;
                    }
                }
                dbCon.Close();
                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
            }
            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (serviceTypeDg.SelectedItems.Count > 0)
            {
                id = (serviceTypeDg.Columns[0].GetCellContent(serviceTypeDg.SelectedItem) as TextBlock).Text;
                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
                MessageBoxResult result = MessageBox.Show("Do you wish to delete this record?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `service_t` SET `isDeleted`= 1 WHERE serviceID = '" + id + "';";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Record successfully deleted!");
                        }
                    }
                    dbCon.Close();
                    serviceTypeList.Visibility = Visibility.Collapsed;
                    serviceTypeAdd.Visibility = Visibility.Visible;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setWindowControls();
        }

        private void serviceTypeDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(serviceTypeDg))
            {
                if (serviceTypeDg.SelectedItems.Count > 0)
                {
                    serviceTypeDg.Columns[4].Visibility = Visibility.Visible;
                    serviceTypeDg.Columns[5].Visibility = Visibility.Visible;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (!visual.IsDescendantOf(serviceTypeDg))
            {
                if (serviceTypeDg.SelectedItems.Count > 0)
                {
                    serviceTypeDg.Columns[4].Visibility = Visibility.Hidden;
                    serviceTypeDg.Columns[5].Visibility = Visibility.Hidden;
                }
            }
        }

        private void setPriceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (locationPrice.Value != null)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                MessageBoxResult result = MessageBox.Show("Do you want to save this price?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    if (initPrice)
                    {
                        string query = "INSERT INTO location_details_t (locationProvinceID,locationPrice) VALUES ('" + custProvinceCb.SelectedValue + "', '" + locationPrice.Value + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Price saved.");
                            custProvinceCb.SelectedValue = -1;
                            locationPrice.Value = 0;

                            serviceName.Clear();
                            serviceDesc.Clear();
                            servicePrice.Value = 0;
                            setWindowControls();
                        }
                    }
                    else
                    {
                        string query = "UPDATE `location_details_t` SET locationPrice = '" + locationPrice.Value + "' WHERE locationId = '" + locationid + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Price updated.");
                            id = "";
                            custProvinceCb.SelectedValue = -1;
                            locationPrice.Value = 0;
                            initPrice = true;
                            setWindowControls();
                        }
                    }
                    


                }
                else if (result == MessageBoxResult.No)
                {
                    custProvinceCb.SelectedValue = -1;
                    locationPrice.Value = 0;
                }
                else if (result == MessageBoxResult.Cancel)
                {
                }
            }
            else
            {
                MessageBox.Show("Please enter the price.");
            }
            
        }
        bool initPrice = true;
        string locationid = "";
        private void custProvinceCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (custProvinceCb.SelectedIndex==-1)
            {
                setPriceBtn.IsEnabled = false;
            }
            else
            {
                setPriceBtn.IsEnabled = true;
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (dbCon.IsConnect())
                {
                    string query = "SELECT locationID,locationPrice FROM location_details_t " +
                        "WHERE locationProvinceId = '" + custProvinceCb.SelectedValue + "';";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    if (fromDbTable.Rows.Count!=0)
                    {
                        initPrice = false;
                        foreach (DataRow dr in fromDbTable.Rows)
                        {
                            locationPrice.Value = Decimal.Parse(dr["locationPrice"].ToString());
                            locationid = dr["locationId"].ToString();
                        }
                    }
                    else
                    {
                        locationPrice.Value = 0;
                        initPrice = true;
                    }
                }
                }
            
        }
    }
}
