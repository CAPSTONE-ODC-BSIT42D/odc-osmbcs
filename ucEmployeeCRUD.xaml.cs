using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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
    /// Interaction logic for ucEmployeeCRUD.xaml
    /// </summary>
    public partial class ucEmployeeCRUD : UserControl
    {
        public ucEmployeeCRUD()
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

        private void editEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in employeeDetailsFormGrid1.Children)
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
            saveCancelGrid1.Visibility = Visibility.Visible;
            editCloseGrid1.Visibility = Visibility.Collapsed;
            MainVM.isEdit = true;
        }

        private void cancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                resetFieldsValue();
                OnSaveCloseButtonClicked(e);
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }

        private void saveRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                foreach (var element in employeeDetailsFormGrid1.Children)
                {
                    if (element is TextBox)
                    {
                        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                        Validation.ClearInvalid(expression);
                        if (((TextBox)element).IsEnabled)
                        {
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((TextBox)element);
                        }
                    }
                    if (element is ComboBox)
                    {
                        BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                        expression.UpdateSource();
                        validationError = Validation.GetHasError((ComboBox)element);
                    }
                }
                if (!validationError)
                {
                    saveDataToDb();
                    OnSaveCloseButtonClicked(e);
                }
                
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

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
                    //MessageBox.Show(ex.Message);
                }

            }
        }

        private void employeeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (employeeType.SelectedIndex == 0)
                {
                    contractorOnlyGrid.Visibility = Visibility.Collapsed;
                    employeeOnlyGrid.Visibility = Visibility.Visible;
                }
                else if (employeeType.SelectedIndex == 1)
                {
                    contractorOnlyGrid.Visibility = Visibility.Visible;
                    employeeOnlyGrid.Visibility = Visibility.Collapsed;
                }
            }
            
        }

        private void contactDetails_Checked(object sender, RoutedEventArgs e)
        {
            string propertyName = ((CheckBox)sender).Name;
            if (propertyName.Equals(empTelCb.Name))
            {
                if ((bool)empTelCb.IsChecked && (bool)empMobCb.IsChecked && (bool)empEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    empTelCb.IsChecked = false;
                }
                else
                {
                    if (empTelephoneTb.IsEnabled)
                    {
                        empTelephoneTb.IsEnabled = false;
                        empTelephoneTb.Text = "";
                    }
                }

            }
            else if (propertyName.Equals(empMobCb.Name))
            {
                if ((bool)empTelCb.IsChecked && (bool)empMobCb.IsChecked && (bool)empEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    empMobCb.IsChecked = false;
                }
                else
                {
                    if (empMobileNumberTb.IsEnabled)
                    {
                        empMobileNumberTb.IsEnabled = false;
                        empMobileNumberTb.Text = "";
                    }
                }
            }
            else if (propertyName.Equals(empEmailCb.Name))
            {
                if ((bool)empTelCb.IsChecked && (bool)empMobCb.IsChecked && (bool)empEmailCb.IsChecked)
                {
                    MessageBox.Show("Atleast one contact information is needed");
                    empEmailCb.IsChecked = false;
                }
                else
                {
                    if (empEmailAddressTb.IsEnabled)
                    {
                        empEmailAddressTb.IsEnabled = false;
                        empEmailAddressTb.Text = "";
                    }
                }
            }
            
        }

        private void contactDetail_Unchecked(object sender, RoutedEventArgs e)
        {
            string propertyName = ((CheckBox)sender).Name;
            if (propertyName.Equals(empTelCb.Name))
            {
                if (!empTelephoneTb.IsEnabled)
                    empTelephoneTb.IsEnabled = true;

            }
            else if (propertyName.Equals(empMobCb.Name))
            {
                if (!empMobileNumberTb.IsEnabled)
                    empMobileNumberTb.IsEnabled = true;
            }
            else if (propertyName.Equals(empEmailCb.Name))
            {
                if (!empEmailAddressTb.IsEnabled)
                    empEmailAddressTb.IsEnabled = true;
            }
            
        }
        MySqlCommand cmd;
        private void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            string empID = "";
            try
            {
                using (MySqlConnection conn = dbCon.Connection)
                {
                    conn.Open();
                    if (!MainVM.isEdit)
                    {
                        
                        cmd = new MySqlCommand("INSERT_EMPLOYEE", conn);
                        cmdParameters();
                        SecureString passwordsalt = empPasswordTb.SecurePassword;
                        foreach (Char c in "$w0rdf!$h")
                        {
                            passwordsalt.AppendChar(c);
                        }
                        passwordsalt.MakeReadOnly();

                        cmd.Parameters.AddWithValue("@upassword", SecureStringToString(passwordsalt));
                        cmd.Parameters["@upassword"].Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new MySqlCommand("UPDATE_EMPLOYEE", conn);
                        cmdParameters();
                        cmd.Parameters.AddWithValue("@empID", MainVM.SelectedEmployeeContractor.EmpID);
                        
                        cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                resetFieldsValue();
                MainVM.isEdit = false;
            }
        }
        void cmdParameters()
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fName", empFirstNameTb.Text);
            cmd.Parameters["@fName"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@lName", empLastNameTb.Text);
            cmd.Parameters["@lName"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@middleInitial", empMiddleInitialTb.Text);
            cmd.Parameters["@middleInitial"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@address", empAddressTb.Text);
            cmd.Parameters["@address"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@city", empCityTb.Text);
            cmd.Parameters["@city"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@provinceID", empProvinceCb.SelectedValue);
            cmd.Parameters["@provinceID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@username", empUserNameTb.Text);
            cmd.Parameters["@username"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@picBLOB", picdata);
            cmd.Parameters["@picBLOB"].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("@sigBLOB", null);
            cmd.Parameters["@sigBLOB"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@positionID", empPostionCb.SelectedValue);
            cmd.Parameters["@positionID"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@empEmail", empEmailAddressTb.Text);
            cmd.Parameters["@empEmail"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@empTelephone", empTelephoneTb.Text);
            cmd.Parameters["@empTelephone"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@empMobile", empMobileNumberTb.Text);
            cmd.Parameters["@empMobile"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@empType", employeeType.SelectedIndex);
            cmd.Parameters["@empType"].Direction = ParameterDirection.Input;



            if (employeeType.SelectedIndex == 1)
            {
                cmd.Parameters.AddWithValue("@jobID", empJobCb.SelectedValue);
                cmd.Parameters["@jobID"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@dateFrom", empDateStarted.Value);
                cmd.Parameters["@dateFrom"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@dateTo", empDateEnded.Value);
                cmd.Parameters["@dateTo"].Direction = ParameterDirection.Input;
            }
            else
            {
                cmd.Parameters.AddWithValue("@jobID", DBNull.Value);
                cmd.Parameters["@jobID"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@dateFrom", DBNull.Value);
                cmd.Parameters["@dateFrom"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@dateTo", DBNull.Value);
                cmd.Parameters["@dateTo"].Direction = ParameterDirection.Input;
            }
        }

        public void loadDataToUi()
        {
            employeeType.SelectedIndex = int.Parse(MainVM.SelectedEmployeeContractor.EmpType);
            empFirstNameTb.Text = MainVM.SelectedEmployeeContractor.EmpFname;
            empLastNameTb.Text = MainVM.SelectedEmployeeContractor.EmpLName;
            empMiddleInitialTb.Text = MainVM.SelectedEmployeeContractor.EmpMiddleInitial;
            empAddressTb.Text = MainVM.SelectedEmployeeContractor.EmpAddress;
            empCityTb.Text = MainVM.SelectedEmployeeContractor.EmpCity;
            empProvinceCb.SelectedValue = MainVM.SelectedEmployeeContractor.EmpProvinceID;
            var stride = (empImage.ActualWidth * PixelFormats.Rgba64.BitsPerPixel + 7) / 8;
            if (MainVM.SelectedEmployeeContractor.EmpPic != null)
                empImage.Source = BitmapSource.Create((int)empImage.Width, (int)empImage.Height, 96, 96, PixelFormats.Rgba64, null, MainVM.SelectedEmployeeContractor.EmpPic, (int)stride);

            if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpTelephone))
            {
                empTelCb.IsChecked = false;
                empTelCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpMobile))
            {
                empMobCb.IsChecked = false;
                empMobCb.IsChecked = true;
            }
            if (String.IsNullOrWhiteSpace(MainVM.SelectedEmployeeContractor.EmpEmail))
            {
                empEmailCb.IsChecked = false;
                empEmailCb.IsChecked = true;
            }
            empTelephoneTb.Text = MainVM.SelectedEmployeeContractor.EmpTelephone;
            empMobileNumberTb.Text = MainVM.SelectedEmployeeContractor.EmpMobile;
            empEmailAddressTb.Text = MainVM.SelectedEmployeeContractor.EmpEmail;
            if (employeeType.SelectedIndex == 0)
            {
                empPostionCb.SelectedValue = MainVM.SelectedEmployeeContractor.PositionID;
                empUserNameTb.Text = MainVM.SelectedEmployeeContractor.EmpUserName;

                empPasswordTb.IsEnabled = false;
            }
            else if (employeeType.SelectedIndex == 1)
            {
                empJobCb.SelectedValue = MainVM.SelectedEmployeeContractor.JobID;
                empDateStarted.Text = MainVM.SelectedEmployeeContractor.EmpDateTo;
                empDateEnded.Text = MainVM.SelectedEmployeeContractor.EmpDateFrom;
            }
        }

        String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        private void resetFieldsValue()
        {
            employeeDetailsFormGridSv.ScrollToTop();
            foreach (var element in employeeDetailsFormGrid1.Children)
            {
                if (element is TextBox)
                {
                    BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                    if (expression != null)
                        Validation.ClearInvalid(expression);
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
            MainVM.StringTextBox = string.Empty;
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainVM.isEdit&&this.IsVisible)
            {
                loadDataToUi();
            }
            employeeType.SelectedIndex = 0;
        }
    }
}
