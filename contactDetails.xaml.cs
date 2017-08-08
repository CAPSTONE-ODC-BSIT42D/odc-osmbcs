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
    /// Interaction logic for contactDetails.xaml
    /// </summary>
    public partial class contactDetails : Window
    {
        private static String dbname = "odc_db";
        public String EmailAddress { get; set; }
        public String PhoneNumber { get; set; }
        public String MobileNumber { get; set; }
        public contactDetails()
        {
            InitializeComponent();
            contactDetailsEmailTb.DataContext = this;
            contactDetailsMobileTb.DataContext = this;
            contactDetailsPhoneTb.DataContext = this;
        }

        private void contactTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactTypeCb.SelectedIndex == 1)
            {
                contactDetailsEmailTb.Visibility = Visibility.Visible;
                contactDetailsEmailTb.IsEnabled = false;
                contactDetailsMobileTb.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb.Visibility = Visibility.Collapsed;
            }
            else if (contactTypeCb.SelectedIndex == 1)
            {
                contactDetailsEmailTb.Visibility = Visibility.Visible;
                contactDetailsMobileTb.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb.Visibility = Visibility.Collapsed;
            }
            else if (contactTypeCb.SelectedIndex == 2)
            {
                contactDetailsEmailTb.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb.Visibility = Visibility.Visible;
                contactDetailsPhoneTb.Visibility = Visibility.Collapsed;
            }
            else if (contactTypeCb.SelectedIndex == 2)
            {
                contactDetailsEmailTb.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb.Visibility = Visibility.Visible;
            }
        }
        public List<string> idOf = new List<string>();
        private string contactDetail = "";
        private void addNewContactBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            string query = "INSERT INTO contact_details_t (contactType,contactData) VALUES ('" + contactTypeCb.SelectedIndex + "','" + contactDetail + "')";
            if (dbCon.insertQuery(query, dbCon.Connection))
            {
                query = "select last_insert_id() from contact_details_t";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                dataAdapter.Fill(fromDb, "t");
                foreach (DataRow myRow in fromDb.Tables[0].Rows)
                {
                    idOf.Add(myRow[0].ToString());
                }
                contactTypeCb.SelectedIndex = 0;
                contactDetailsEmailTb.Text = "";
                contactDetailsMobileTb.Text = "";
                contactDetailsPhoneTb.Text = "";
                contactDetail = "";
                MessageBox.Show("Contact successfull added!");
            }
        }
        
        private void contactDetailsEmailTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb) == true)
                saveBtn.IsEnabled = false;
            else
            {
                contactDetail = contactDetailsEmailTb.Text;
                validateTextBoxes();
            }
        }

        private void contactDetailsPhoneTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb) == true)
                saveBtn.IsEnabled = false;
            else
            {
                contactDetail = contactDetailsPhoneTb.Text;
                validateTextBoxes();
            }
        }

        private void contactDetailsMobileTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb) == true)
                saveBtn.IsEnabled = false;
            else
            {
                contactDetail = contactDetailsMobileTb.Text;
                validateTextBoxes();
            }
        }

        private void validateTextBoxes()
        {
            if (idOf.Count == 0)
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
