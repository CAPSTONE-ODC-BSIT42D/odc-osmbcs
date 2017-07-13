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
    /// Interaction logic for representativeWindow.xaml
    /// </summary>
    public partial class RepresentativeWindow : Window
    {
        
        public RepresentativeWindow()
        {
            InitializeComponent();
            this.DataContext = MainMenu.MainVM;
        }
        public string custName { get; set; }
        public string custId { get; set; }
        public string repType { get; set; }
        private void setControlsValue()
        {
            contactTypeCb.SelectedIndex = 0;
            if (repDetails != null && idOfContacts !=0)
            {
                firstNameTb.Text = repDetails[0];
                middleInitialTb.Text = repDetails[1];
                lastNameTb.Text = repDetails[2];
                MainMenu.MainVM.RepContacts = MainMenu.MainVM.ContactOfRep[idOfContacts];
            }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setControlsValue();
        }
        private static String dbname = "odc_db";
        public String[] repDetails;
        public List<string[]> contactDetails = new List<string[]>();
        public int idOfContacts;
        private string contactDetail = "";
        private void addNewCustContactBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (contactTypeCb.SelectedIndex != 0)
            {
                MainMenu.MainVM.RepContacts.Add(new Contact() { ContactTypeID = contactTypeCb.SelectedIndex.ToString(), ContactType = contactTypeCb.SelectedValue.ToString(), ContactDetails = contactDetail });
                validateTextBoxes();
                contactDetailsEmailTb.Text = "";
                contactDetailsMobileTb.Text = "";
                contactDetailsPhoneTb.Text = "";
                Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
                Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
                validateTextBoxes();
            }
            else
            {
                MessageBox.Show("Select The Type");
            }
        }

        private void contactTypeCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactTypeCb.SelectedIndex == 0)
            {
                contactDetailsEmailTb.IsEnabled = false;
                contactDetailsMobileTb.IsEnabled = false;
                contactDetailsPhoneTb.IsEnabled = false;
            }
            else if (contactTypeCb.SelectedIndex == 1)
            {
                contactDetailsEmailTb.Visibility = Visibility.Visible;
                contactDetailsMobileTb.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb.Visibility = Visibility.Collapsed;
                contactDetailsEmailTb.IsEnabled = true;
                contactDetailsMobileTb.IsEnabled = false;
                contactDetailsPhoneTb.IsEnabled = false;
            }
            else if (contactTypeCb.SelectedIndex == 2)
            {
                contactDetailsEmailTb.Visibility = Visibility.Collapsed;
                contactDetailsPhoneTb.Visibility = Visibility.Visible;
                contactDetailsMobileTb.Visibility = Visibility.Collapsed;
                contactDetailsEmailTb.IsEnabled = false;
                contactDetailsMobileTb.IsEnabled = false;
                contactDetailsPhoneTb.IsEnabled = true;
            }
            else if (contactTypeCb.SelectedIndex == 3)
            {
                contactDetailsEmailTb.Visibility = Visibility.Collapsed;
                contactDetailsMobileTb.Visibility = Visibility.Visible;
                contactDetailsPhoneTb.Visibility = Visibility.Collapsed;
                contactDetailsEmailTb.IsEnabled = false;
                contactDetailsMobileTb.IsEnabled = true;
                contactDetailsPhoneTb.IsEnabled = false;
            }
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
            if (MainMenu.MainVM.RepContacts.Count>0)
            {
                saveBtn.IsEnabled = true;
                saveCustContactBtn.IsEnabled = true;
            }
            else
            {
                saveBtn.IsEnabled = false;
                saveCustContactBtn.IsEnabled = false;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            repDetails =  new String[] { firstNameTb.Text, middleInitialTb.Text, lastNameTb.Text };
            MainMenu.MainVM.ContactOfRep.Add(MainMenu.MainVM.RepContacts);
            MainMenu.MainVM.RepContacts.Clear();
            this.Close();
        }


        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void editRepContBtn_Click(object sender, RoutedEventArgs e)
        {
            if (repContactsDg.SelectedItem != null)
            {
                contactTypeCb.SelectedIndex = int.Parse(MainMenu.MainVM.SelectedRepContact.ContactTypeID);

                if (MainMenu.MainVM.SelectedRepContact.ContactTypeID.Equals("1"))
                {
                    contactDetailsEmailTb.Text = MainMenu.MainVM.SelectedRepContact.ContactDetails;

                }
                else if (MainMenu.MainVM.SelectedRepContact.ContactTypeID.Equals("2"))
                {
                    contactDetailsPhoneTb.Text = MainMenu.MainVM.SelectedRepContact.ContactDetails;
                }
                else if (MainMenu.MainVM.SelectedRepContact.ContactTypeID.Equals("3"))
                {
                    contactDetailsMobileTb.Text = MainMenu.MainVM.SelectedRepContact.ContactDetails;
                }
                saveCustContactBtn.Visibility = Visibility.Visible;
                cancelCustContactBtn.Visibility = Visibility.Visible;
            }
        }

        private void delRepContBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete this contact information?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                MainMenu.MainVM.RepContacts.Remove(MainMenu.MainVM.SelectedRepContact);
            }
            else if (result == MessageBoxResult.No)
            {
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
        }

        private void saveCustContactBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(System.Windows.Controls.Validation.GetHasError(contactDetailsPhoneTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsEmailTb) == true) && !(System.Windows.Controls.Validation.GetHasError(contactDetailsMobileTb) == true))
            {
                if (contactTypeCb.SelectedIndex != 0)
                {
                    saveCustContactBtn.Visibility = Visibility.Hidden;
                    MainMenu.MainVM.SelectedRepContact.ContactDetails = contactDetail;
                    MainMenu.MainVM.SelectedRepContact.ContactType = contactTypeCb.SelectedValue.ToString();
                    MainMenu.MainVM.SelectedRepContact.ContactTypeID = contactTypeCb.SelectedIndex.ToString();
                    contactDetailsEmailTb.Text = "";
                    contactDetailsMobileTb.Text = "";
                    contactDetailsPhoneTb.Text = "";
                    cancelCustContactBtn.Visibility = Visibility.Hidden;
                    saveCustContactBtn.Visibility = Visibility.Hidden;
                    Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
                    Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
                    Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
                }
                else
                {
                    MessageBox.Show("Select The Type");
                }
            }
            else
                MessageBox.Show("Please resolve the error first.");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (!visual.IsDescendantOf(repContactsDg))
            {
                repContactsDg.SelectedIndex = -1;
                repContactsDg.Columns[repContactsDg.Columns.IndexOf(colEditRepContact)].Visibility = Visibility.Hidden;
                repContactsDg.Columns[repContactsDg.Columns.IndexOf(colDelRepContact)].Visibility = Visibility.Hidden;
            }
        }

        private void repContactsDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Visual visual = e.OriginalSource as Visual;
            if (visual.IsDescendantOf(repContactsDg))
            {
                if (repContactsDg.SelectedItems.Count > 0)
                {
                    repContactsDg.Columns[repContactsDg.Columns.IndexOf(colEditRepContact)].Visibility = Visibility.Visible;
                    repContactsDg.Columns[repContactsDg.Columns.IndexOf(colDelRepContact)].Visibility = Visibility.Visible;
                }
            }
        }

        private void cancelCustContactBtn_Click(object sender, RoutedEventArgs e)
        {
            contactDetailsEmailTb.Text = "";
            contactDetailsMobileTb.Text = "";
            contactDetailsPhoneTb.Text = "";
            cancelCustContactBtn.Visibility = Visibility.Hidden;
            saveCustContactBtn.Visibility = Visibility.Hidden;
            Validation.ClearInvalid((contactDetailsPhoneTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsEmailTb).GetBindingExpression(TextBox.TextProperty));
            Validation.ClearInvalid((contactDetailsMobileTb).GetBindingExpression(TextBox.TextProperty));
        }
    }
}
