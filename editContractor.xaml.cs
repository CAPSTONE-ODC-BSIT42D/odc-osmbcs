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
    /// Interaction logic for editEmployee.xaml
    /// </summary>
    public partial class editContractor : Window
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
        String Empid = "";
        String locId = "";
        List<Position> position = new List<Position>();
        public editContractor(String id)
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
            Empid = id;
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
                provinceCb.ItemsSource = fromDb.Tables["t"].DefaultView;
                dbCon.Close();

            }
        }

        private void setControlValuesSynced(String id)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {

                string query = "SELECT e.contID,e.contFName,e.contMi,e.contLname,e.jobTitle, e.contContact, e.contEmail, e.locationId, e.dateEnded, e.dateStarted, ld.locationAddress,ld.locationCityID, p.locProvinceID,pic.contPic,pic.contSignature " +
                    "FROM contractor_t e " +
                    "JOIN location_details_t ld ON e.locationID = ld.locationID " +
                    "JOIN provinces_t p ON ld.locationProvinceID = p.locProvinceId " +
                    "JOIN cont_pic_t pic ON e.contID = pic.contID WHERE e.contID = '"+id+"';";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    try
                    {
                        locId = dr["locationId"].ToString();
                        FirstName = dr["contFName"].ToString();
                        MiddleName = dr["contMi"].ToString();
                        LastName = dr["contLname"].ToString();
                        byte[] data = { };
                        if (!Convert.IsDBNull(dr["contPic"]))
                        {
                            data = (byte[])dr["contPic"];
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
                            {
                                var imageSource = new BitmapImage();
                                imageSource.BeginInit();
                                imageSource.StreamSource = ms;
                                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                                imageSource.EndInit();
                                // Assign the Source property of your image
                                empImage.Source = imageSource;
                            }
                        }
                        Address = dr["locationAddress"].ToString();
                        int locProvId = Int32.Parse(dr["locProvinceID"].ToString());
                        provinceCb.SelectedIndex = locProvId - 1;
                        Address = dr["locationAddress"].ToString();
                        int locCityId = Int32.Parse(dr["locationCityID"].ToString());
                        cityCb.SelectedValue = locCityId;
                        Number = dr["contContact"].ToString();
                        Email = dr["contEmail"].ToString();
                        JobTitle = dr["jobTitle"].ToString();
                        dateStarted.Text = dr["dateStarted"].ToString();
                        dateEnded.Text = dr["dateStarted"].ToString();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
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
            MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE `location_details_t` SET locationAddress = '"+ addressTb.Text + "',locationCityID = '" + cityCb.SelectedValue + "', locationProvinceId = '" + provinceCb.SelectedValue + "' WHERE locationID = '"+locId+"'";

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        
                        string query1 = "UPDATE `contractor_t` SET contFname = '" + firstNameTb.Text + "',contLname = '" + lastNameTb.Text + "', contMI = '" + middleInitialTb.Text + "', contEmail = '" + emailAddressTb.Text + "', contContact = '" + mobileNumberTb.Text + "', jobTitle = '" + jobTitleTb.Text + "', dateStarted = '" + dateStarted.Text + "', dateEnded = '" + dateEnded.Text + "' WHERE contID = '" + Empid + "'";
                        if (dbCon.insertQuery(query1, dbCon.Connection))
                        {
                            try
                            {
                                string connstring = string.Format("Server=localhost; database={0}; UID=root; password=password", dbname);
                                MySqlConnection conn = new MySqlConnection(connstring);
                                conn.Open();
                                MySqlCommand cmd = new MySqlCommand("UPDATE cont_pic_t SET contPic = @picture ,contSignature = @signature WHERE contID = '"+Empid+"'", conn);
                                cmd.Parameters.Add("@picture", MySqlDbType.LongBlob);
                                cmd.Parameters["@picture"].Value = picdata;
                                cmd.Parameters.Add("@signature", MySqlDbType.MediumBlob);
                                cmd.Parameters["@signature"].Value = "";
                                cmd.ExecuteNonQuery();
                                conn.Close();
                                MessageBox.Show("Changes Saved.");
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
    public class Position
    {
        public string Name { get; set; }
    }
}
