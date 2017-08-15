using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// Interaction logic for addEmployee.xaml
    /// </summary>
    public partial class addContractor : Window
    {
        public String FirstName { get; set; }
        public String MiddleName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String Number { get; set; }
        public String Email { get; set; }
        public object locProvinceId { get; set; }
        public String JobTitle { get; set; }
        public object cityID { get; set; }
        public addContractor()
        {
            InitializeComponent();
            firstNameTb.DataContext = this;
            middleInitialTb.DataContext = this;
            lastNameTb.DataContext = this;
            addressTb.DataContext = this;
            cityCb.DataContext = this;
            mobileNumberTb.DataContext = this;
            emailAddressTb.DataContext = this;
            provinceCb.DataContext = this;
            jobTitleTb.DataContext = this;

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
                provinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setControlsValues();
        }
        byte[] picdata;
        byte[] sigdata;
        private void openFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    empImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    picdata = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            MessageBoxResult result = MessageBox.Show("Do you want to save this new contractor?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO location_details_t (locationAddress,locationCityID,locationProvinceID) VALUES ('" + addressTb.Text + "','" + cityCb.SelectedValue+ "', '" + provinceCb.SelectedValue + "')";

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
                        string pass = RandomString(9);
                        string query1 = "INSERT INTO contractor_t (contFname,contlname,contMI,contEmail,contContact,password,locationID,dateStarted,dateEnded,jobTitle) VALUES ('" + firstNameTb.Text + "','" + lastNameTb.Text + "','" + middleInitialTb.Text + "','" + emailAddressTb.Text + "','" + mobileNumberTb.Text + "','" + pass + "','" + locID + "','" + dateStarted.Text + "','" + dateEnded.Text + "','" + jobTitleTb.Text + "')";
                        if (dbCon.insertQuery(query1, dbCon.Connection))
                        {
                            query = "select last_insert_id() from contractor_t";
                            dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                            fromDb = new DataSet();
                            dataAdapter.Fill(fromDb, "t");
                            string contID = "";
                            foreach (DataRow myRow in fromDb.Tables[0].Rows)
                            {
                                contID = myRow[0].ToString();
                            }
                            try
                            {
                                string connstring = string.Format("Server=localhost; database={0}; UID=root; password=password", dbname);
                                MySqlConnection conn = new MySqlConnection(connstring);
                                conn.Open();
                                MySqlCommand cmd = new MySqlCommand("insert into cont_pic_t(contPic,contSignature,contId)" +
                                    " values(@picture,@signature,'"+ contID + "')", conn);
                                cmd.Parameters.Add("@picture", MySqlDbType.LongBlob);
                                cmd.Parameters["@picture"].Value = picdata;
                                cmd.Parameters.Add("@signature", MySqlDbType.MediumBlob);
                                cmd.Parameters["@signature"].Value = "";
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                MessageBox.Show("Contractor record successfully saved");
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
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

        

        Random random = new Random();
        private string RandomString(int Size)
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[random.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }
        
        private byte[] getSig(string fileName)
        {
            byte[] bitmapBytes = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            empImage.Source = new BitmapImage(new Uri(fileName));
            bitmapBytes = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return bitmapBytes;
        }

        private void firstNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(firstNameTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void middleInitialTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(middleInitialTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void lastNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(lastNameTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void addressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(addressTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void cityTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(cityCb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void provinceCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect() && provinceCb.SelectedIndex != -1)
            {
                string query = "SELECT * FROM city_by_province_t cp WHERE provinceID = '" + provinceCb.SelectedValue + "'";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                cityCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();
            }
            if (System.Windows.Controls.Validation.GetHasError(provinceCb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void mobileNumberTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(mobileNumberTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void emailAddressTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(emailAddressTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void jobTitleTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(jobTitleTb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }

        private void cityCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(cityCb) == true)
                saveBtn.IsEnabled = false;
            else validateTextBoxes();
        }
        private void dateStarted_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            validateTextBoxes();
        }

        private void dateEnded_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            validateTextBoxes();
        }
        private void validateTextBoxes()
        {
            if (firstNameTb.Text.Equals("") || lastNameTb.Text.Equals("") || middleInitialTb.Text.Equals("") || provinceCb.SelectedIndex == -1 || cityCb.SelectedIndex == -1 || emailAddressTb.Text.Equals("") || mobileNumberTb.Text.Equals("") || jobTitleTb.Text.Equals("") || dateStarted.Text.Equals("") || dateEnded.Text.Equals(""))
            {
                saveBtn.IsEnabled = false;
            }
            else
            {
                saveBtn.IsEnabled = true;
            }
        }
        

        
    }
}
