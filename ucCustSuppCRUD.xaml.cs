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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucCustSuppCRUD.xaml
    /// </summary>
    public partial class ucCustSuppCRUD : UserControl
    {
        public ucCustSuppCRUD()
        {
            InitializeComponent();
        }

        private bool validationError = false;
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void contactDetails_Checked(object sender, RoutedEventArgs e)
        {
            string propertyName = ((CheckBox)sender).Name;

            if (propertyName.Equals(companyTelCb.Name))
            {
                if ((bool)companyTelCb.IsChecked && (bool)companyMobCb.IsChecked && (bool)companyEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    companyTelCb.IsChecked = false;
                }
                else
                {
                    if (companyTelephoneTb.IsEnabled)
                    {
                        companyTelephoneTb.IsEnabled = false;
                        companyTelephoneTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(companyMobCb.Name))
            {
                if ((bool)companyTelCb.IsChecked && (bool)companyMobCb.IsChecked && (bool)companyEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    companyMobCb.IsChecked = false;
                }
                else
                {
                    if (companyMobileTb.IsEnabled)
                    {
                        companyMobileTb.IsEnabled = false;
                        companyMobileTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(companyEmailCb.Name))
            {
                if ((bool)companyTelCb.IsChecked && (bool)companyMobCb.IsChecked && (bool)companyEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    companyEmailCb.IsChecked = false;
                }
                else
                {
                    if (companyEmailTb.IsEnabled)
                    {
                        companyEmailTb.IsEnabled = false;
                        companyEmailTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(repTelCb.Name))
            {
                if ((bool)repTelCb.IsChecked && (bool)repMobCb.IsChecked && (bool)repEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    repTelCb.IsChecked = false;
                }
                else
                {
                    if (repTelephoneTb.IsEnabled)
                    {
                        repTelephoneTb.IsEnabled = false;
                        repTelephoneTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(repMobCb.Name))
            {
                if ((bool)repTelCb.IsChecked && (bool)repMobCb.IsChecked && (bool)repEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    repMobCb.IsChecked = false;
                }
                else
                {
                    if (repMobileTb.IsEnabled)
                    {
                        repMobileTb.IsEnabled = false;
                        repMobileTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(repEmailCb.Name))
            {
                if ((bool)repTelCb.IsChecked && (bool)repMobCb.IsChecked && (bool)repEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    repEmailCb.IsChecked = false;
                }
                else
                {
                    if (repEmailTb.IsEnabled)
                    {
                        repEmailTb.IsEnabled = false;
                        repEmailTb.Text = "";
                    }
                }
            }
        }

        private void contactDetail_Unchecked(object sender, RoutedEventArgs e)
        {
            string propertyName = ((CheckBox)sender).Name;
            if (propertyName.Equals(companyTelCb.Name))
            {
                if (!companyTelephoneTb.IsEnabled)
                    companyTelephoneTb.IsEnabled = true;
            }
            else if (propertyName.Equals(companyMobCb.Name))
            {
                if (!companyMobileTb.IsEnabled)
                    companyMobileTb.IsEnabled = true;
            }
            else if (propertyName.Equals(companyEmailCb.Name))
            {
                if (!companyEmailTb.IsEnabled)
                    companyEmailTb.IsEnabled = true;
            }
            else if (propertyName.Equals(repTelCb.Name))
            {
                if (!repTelephoneTb.IsEnabled)
                    repTelephoneTb.IsEnabled = true;
            }
            else if (propertyName.Equals(repMobCb.Name))
            {
                if (!repMobileTb.IsEnabled)
                    repMobileTb.IsEnabled = true;
            }
            else if (propertyName.Equals(repEmailCb.Name))
            {
                if (!repEmailTb.IsEnabled)
                    repEmailTb.IsEnabled = true;
            }
        }

        private void editCompanyBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in companyDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {

                    ((TextBox)element).IsEnabled = true;
                }
                else if (element is ComboBox)
                {
                    ((ComboBox)element).IsEnabled = true;
                }
                else if (element is CheckBox)
                {
                    ((CheckBox)element).IsEnabled = true;
                }
            }
            loadDataToUi();
            editCloseGrid.Visibility = Visibility.Collapsed;
            saveCancelGrid.Visibility = Visibility.Visible;
            MainVM.isEdit = true;
        }

        private void saveRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                foreach (var element in companyDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                        if (expression != null)
                        {
                            if (((TextBox)element).IsEnabled)
                            {
                                expression.UpdateSource();
                                if (Validation.GetHasError((TextBox)element))
                                    validationError = true;
                            }
                        }
                    }
                    if (element is ComboBox)
                    {
                        BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                        expression.UpdateSource();
                        if (Validation.GetHasError((ComboBox)element))
                            validationError = true;
                    }
                }
                if (!validationError)
                {
                    saveDataToDb();
                    OnSaveCloseButtonClicked(e);
                }
                else
                {
                    MessageBox.Show("Resolve the error first");
                    validationError = false;
                }
                    
            }
            else if (result == MessageBoxResult.Cancel)
            {

            }

        }

        private void cancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                resetFieldsValue();
                OnSaveCloseButtonClicked(e);
            }
            else if (result == MessageBoxResult.No)
            {
            }

        }

        private void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            string repID = "";
            MySqlCommand cmd;
            try
            {
                using (MySqlConnection conn = dbCon.Connection)
                {
                    conn.Open();

                    if (!MainVM.isEdit)
                    {
                        cmd = new MySqlCommand("INSERT_COMPANY", conn);
                    }
                    else
                    {
                        cmd = new MySqlCommand("UPDATE_COMPANY", conn);
                    }
                    //INSERT NEW CUSTOMER TO DB;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@companyName", companyNameTb.Text);
                    cmd.Parameters["@companyName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@addInfo", companyDescriptionTb.Text);
                    cmd.Parameters["@addInfo"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@address", companyAddressTb.Text);
                    cmd.Parameters["@address"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@city", companyCityTb.Text);
                    cmd.Parameters["@city"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@province", companyProvinceCb.SelectedValue);
                    cmd.Parameters["@province"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@companyPostalCode", companyPostalCode.Text);
                    cmd.Parameters["@companyPostalCode"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@companyEmail", companyEmailTb.Text);
                    cmd.Parameters["@companyEmail"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@companyTelephone", companyTelephoneTb.Text);
                    cmd.Parameters["@companyTelephone"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@companyMobile", companyMobileTb.Text);
                    cmd.Parameters["@companyMobile"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@repTitle", representativeTitle.Text);
                    cmd.Parameters["@repTitle"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@repLName", repLastNameTb.Text);
                    cmd.Parameters["@repLName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@repFName", repFirstNameTb.Text);
                    cmd.Parameters["@repFName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@repMName", repMiddleInitialTb.Text);
                    cmd.Parameters["@repMName"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@repEmail", repEmailTb.Text);
                    cmd.Parameters["@repEmail"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@repTelephone", repTelephoneTb.Text);
                    cmd.Parameters["@repTelephone"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@repMobile", repMobileTb.Text);
                    cmd.Parameters["@repMobile"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@compType", "1");
                    cmd.Parameters["@compType"].Direction = ParameterDirection.Input;
                    if (MainVM.isEdit)
                    {
                        cmd.Parameters.AddWithValue("@compID", MainVM.SelectedCustomerSupplier.CompanyID);
                        cmd.Parameters["@compID"].Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                    else
                        cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                resetFieldsValue();
            }
        }

       
        private void loadDataToUi()
        {
            companyNameTb.Text = MainVM.SelectedCustomerSupplier.CompanyName;
            companyDescriptionTb.Text = MainVM.SelectedCustomerSupplier.CompanyDesc;
            companyAddressTb.Text = MainVM.SelectedCustomerSupplier.CompanyAddress;
            companyCityTb.Text = MainVM.SelectedCustomerSupplier.CompanyCity;
            companyProvinceCb.SelectedValue = MainVM.SelectedCustomerSupplier.CompanyProvinceID;
            companyPostalCode.Text = MainVM.SelectedCustomerSupplier.CompanyPostalCode;
            companyEmailTb.Text = MainVM.SelectedCustomerSupplier.CompanyEmail;
            companyTelephoneTb.Text = MainVM.SelectedCustomerSupplier.CompanyTelephone;
            companyMobileTb.Text = MainVM.SelectedCustomerSupplier.CompanyMobile;
            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyTelephone))
            {
                companyTelCb.IsChecked = false;
                companyTelCb.IsChecked = true;
            }

            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyMobile))
            {
                companyMobCb.IsChecked = false;
                companyMobCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.CompanyEmail))
            {
                companyEmailCb.IsChecked = false;
                companyEmailCb.IsChecked = true;
            }
            representativeTitle.Text = MainVM.SelectedCustomerSupplier.RepTitle;
            repFirstNameTb.Text = MainVM.SelectedCustomerSupplier.RepFirstName;
            repMiddleInitialTb.Text = MainVM.SelectedCustomerSupplier.RepMiddleName;
            repLastNameTb.Text = MainVM.SelectedCustomerSupplier.RepLastName;
            repEmailTb.Text = MainVM.SelectedCustomerSupplier.RepEmail;
            repTelephoneTb.Text = MainVM.SelectedCustomerSupplier.RepTelephone;
            repMobileTb.Text = MainVM.SelectedCustomerSupplier.RepMobile;
            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.RepTelephone))
            {
                repTelCb.IsChecked = false;
                repTelCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.RepMobile))
            {
                repMobCb.IsChecked = false;
                repMobCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedCustomerSupplier.RepEmail))
            {
                repEmailCb.IsChecked = false;
                repEmailCb.IsChecked = true;
            }
        }

        private void resetFieldsValue()
        {
            companyDetailsFormGridSv.ScrollToTop();
            foreach (var element in companyDetailsFormGrid1.Children)
            {

                if (element is TextBox)
                {
                    TextBox tb = element as TextBox;
                    BindingExpression expression = tb.GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    tb.Text = string.Empty;
                }
                else if (element is ComboBox)
                {
                    BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
                    ((ComboBox)element).SelectedIndex = -1;
                }
                else if (element is CheckBox)
                {
                    ((CheckBox)element).IsChecked = false;
                }
            }
            MainVM.isEdit = false;
        }


        private void UserControl_IsVisibleChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainVM.isEdit && this.IsVisible)
            {
                loadDataToUi();
            }
        }
    }
}
