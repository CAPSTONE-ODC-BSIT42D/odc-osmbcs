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
                    else if (element is ComboBox)
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
                    resetFieldsValue();
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
        MySqlCommand cmd;
        private void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            try
            {
                using (MySqlConnection conn = dbCon.Connection)
                {
                    conn.Open();
                    if (!MainVM.isEdit)
                    {
                        
                        cmd = new MySqlCommand("INSERT_EMPLOYEE", conn);
                        cmdParameters();
                        string passwordsalt = empLastNameTb.Text + "$w0rdf!$h";
                        cmd.Parameters.AddWithValue("@upassword", passwordsalt);
                        cmd.Parameters["@upassword"].Direction = ParameterDirection.Input;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new MySqlCommand("UPDATE_EMPLOYEE", conn);
                        cmdParameters();
                        cmd.Parameters.AddWithValue("@empIDinput", MainVM.SelectedEmployeeContractor.EmpID);
                        cmd.Parameters["@empIDinput"].Direction = ParameterDirection.Input;
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
            
            cmd.Parameters.AddWithValue("@username", empUserNameTb.Text);
            cmd.Parameters["@username"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@positionID", empPostionCb.SelectedValue);
            cmd.Parameters["@positionID"].Direction = ParameterDirection.Input;

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
            employeeType.SelectedIndex = MainVM.SelectedEmployeeContractor.EmpType;
            empFirstNameTb.Text = MainVM.SelectedEmployeeContractor.EmpFname;
            empLastNameTb.Text = MainVM.SelectedEmployeeContractor.EmpLName;
            empMiddleInitialTb.Text = MainVM.SelectedEmployeeContractor.EmpMiddleInitial;
            
            if (employeeType.SelectedIndex == 0)
            {
                empPostionCb.SelectedValue = MainVM.SelectedEmployeeContractor.PositionID;
                empUserNameTb.Text = MainVM.SelectedEmployeeContractor.EmpUserName;
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
                    ((TextBox)element).Text = "";
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

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            resetFieldsValue();
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
