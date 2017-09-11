using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : Window
    {
        public MainScreen()
        {
            InitializeComponent();
            
        }
        public static Commands commands = new Commands();
        private static MainViewModel MainVM = new MainViewModel();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            dashboardGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
            loadDataToUi();
        }

        private async void loadDataToUi()
        {
            var slowTask = Task<MainViewModel>.Factory.StartNew(() => commands.FillViewModels());
            await slowTask;
            MainVM.Customers = slowTask.Result.Customers;
            MainVM.Suppliers = slowTask.Result.Suppliers;
            MainVM.Employees = slowTask.Result.Employees;
            MainVM.Contractor = slowTask.Result.Contractor;
            MainVM.ContJobTitle = slowTask.Result.ContJobTitle;
            MainVM.EmpPosition = slowTask.Result.EmpPosition;
            MainVM.ProductList = slowTask.Result.ProductList;
            MainVM.ProductCategory = slowTask.Result.ProductCategory;
            MainVM.ServicesList = slowTask.Result.ServicesList;
            MainVM.Provinces = slowTask.Result.Provinces;

            MainVM.AllCustomerSupplier.Clear();
            foreach(var obj in MainVM.Customers)
            {
                MainVM.AllCustomerSupplier.Add(obj);
            }

            foreach (var obj in MainVM.Suppliers)
            {
                MainVM.AllCustomerSupplier.Add(obj);
            }

            this.DataContext = MainVM;
        }

        private void dashBoardBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            dashboardGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
        }

        private void salesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (salesBtnSubMenuGrid.IsVisible)
                salesBtnSubMenuGrid.Visibility = Visibility.Collapsed;
            else
                salesBtnSubMenuGrid.Visibility = Visibility.Visible;
        }

        private void serviceBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reportsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void manageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (manageSubMenugrid.IsVisible)
                manageSubMenugrid.Visibility = Visibility.Collapsed;
            else
                manageSubMenugrid.Visibility = Visibility.Visible;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in settingsGridStackPanel.Children)
            {
                if (obj is Grid)
                {
                    ((Grid)obj).Visibility = Visibility.Collapsed;
                }
            }
            if (manageEmployeeGrid.IsVisible)
            {
                employeeSettings1.Visibility = Visibility.Visible;
                employeeSettings2.Visibility = Visibility.Visible;
            }
            else if (manageProductListGrid.IsVisible)
            {
                productSettings1.Visibility = Visibility.Visible;
            }
            settingGridBg.Visibility = Visibility.Visible;
        }

        private void closeSideMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            settingGridBg.Visibility = Visibility.Collapsed;
        }

        private void quotesSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ordersSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void invoiceSalesMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void empContManageBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            maintenanceGrid.Visibility = Visibility.Visible;
            foreach (var obj in maintenanceGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            manageEmployeeGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Visible;
            headerLbl.Content = "Manage Employee/Contractor";
        }

        private void custSuppManageBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            maintenanceGrid.Visibility = Visibility.Visible;
            foreach (var obj in maintenanceGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            manageCustomerGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Hidden;
            headerLbl.Content = "Manage Customer/Supplier";
        }

        private void productManageBtn_Click(object sender, RoutedEventArgs e)
        {
            headerLbl.Content = "Manage Product List";
            foreach (var obj in containerGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            maintenanceGrid.Visibility = Visibility.Visible;
            foreach (var obj in maintenanceGrid.Children)
            {
                ((Grid)obj).Visibility = Visibility.Collapsed;
            }
            manageProductListGrid.Visibility = Visibility.Visible;
            settingsBtn.Visibility = Visibility.Visible;
        }

        private void settingsManageMenuBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        //Maintenance

        private void dataGridType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                if (manageEmployeeGrid.IsVisible)
                {
                    if (compType.SelectedIndex == 0)
                    {
                        manageCustomerDataGrid.ItemsSource = MainVM.AllCustomerSupplier;
                    }
                    else if (compType.SelectedIndex == 1)
                    {
                        manageCustomerDataGrid.ItemsSource = MainVM.Customers;
                    }
                    else if (compType.SelectedIndex == 2)
                    {
                        manageCustomerDataGrid.ItemsSource = MainVM.Suppliers;
                    }
                }
                else if (manageEmployeeGrid.IsVisible)
                {
                    if (empType.SelectedIndex == 0)
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.AllEmployeesContractor;
                    }
                    else if (empType.SelectedIndex == 1)
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.Employees;
                    }
                    else if (empType.SelectedIndex == 2)
                    {
                        manageEmployeeDataGrid.ItemsSource = MainVM.Contractor;
                    }
                }
            }
        }


        private void manageEmployeeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(employeeDetailsFormGridBg);
            companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            if (employeeDetailsFormGridBg.IsVisible)
            {
                employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            }
            else
                employeeDetailsFormGridBg.Visibility = Visibility.Visible;
        }

        private void manageCustomerAddBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(companyDetailsFormGrid);
            employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            if (companyDetailsFormGrid.IsVisible)
            {
                companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            }
            else
                companyDetailsFormGridBg.Visibility = Visibility.Visible;
        }
        private bool validationError = false;

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

        private void saveRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (companyDetailsFormGridBg.IsVisible)
                {
                    foreach (var element in companyDetailsFormGrid.Children)
                    {
                        if (element is TextBox)
                        {
                            BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((TextBox)element);
                        }
                        if (element is ComboBox)
                        {
                            BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((ComboBox)element);
                        }
                    }
                }
                else if (employeeDetailsFormGridBg.IsVisible)
                {
                    foreach (var element in employeeDetailsFormGrid.Children)
                    {
                        if (element is TextBox)
                        {
                            BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((TextBox)element);
                        }
                        if (element is ComboBox)
                        {
                            BindingExpression expression = ((ComboBox)element).GetBindingExpression(ComboBox.SelectedItemProperty);
                            expression.UpdateSource();
                            validationError = Validation.GetHasError((ComboBox)element);
                        }
                    }
                }
                if (!validationError)
                    saveDataToDb();
            }
            else if (result == MessageBoxResult.No)
            {
            }
            
        }
        
        private void cancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                resetFieldsValue();
            }
            else if (result == MessageBoxResult.No)
            {
            }
            
        }

        private bool isEdit = false;
        private void saveDataToDb()
        {
            
            if (companyDetailsFormGridBg.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                string[] proc = { "", "", "", "" };

                if (!isEdit)
                {
                    proc[0] = "INSERT_COMPANY";
                    proc[1] = "INSERT_REP";
                }
                else
                {
                    proc[0] = "UPDATE_COMPANY";
                    proc[1] = "UPDATE_REP";
                }
                try
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand(proc[0], conn);
                        string repID = "";
                        cmd = new MySqlCommand(proc[1], conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@repLName", repLastNameTb.Text);
                        cmd.Parameters["@repLName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repFName", repFirstNameTb.Text);
                        cmd.Parameters["@repFName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repMName", repMiddleInitialTb.Text);
                        cmd.Parameters["@repMName"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repEmail", repMiddleInitialTb.Text);
                        cmd.Parameters["@repEmail"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repTelephone", repMiddleInitialTb.Text);
                        cmd.Parameters["@repTelephone"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@repMobile", repMiddleInitialTb.Text);
                        cmd.Parameters["@repMobile"].Direction = ParameterDirection.Input;
                        if (!isEdit)
                        { 
                            cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                            cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            repID = cmd.Parameters["@insertedid"].Value.ToString();
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@repID", MainVM.SelectedCustomerSupplier.RepresentativeID);
                            cmd.Parameters["@repID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }

                        cmd = new MySqlCommand(proc[0], conn);
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

                        cmd.Parameters.AddWithValue("@companyEmail", companyProvinceCb.SelectedValue);
                        cmd.Parameters["@companyEmail"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@companyTelephone", companyProvinceCb.SelectedValue);
                        cmd.Parameters["@companyTelephone"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@companyMobile", companyProvinceCb.SelectedValue);
                        cmd.Parameters["@companyMobile"].Direction = ParameterDirection.Input;

                        cmd.Parameters.AddWithValue("@compType", companyTypeCb.SelectedIndex);
                        cmd.Parameters["@compType"].Direction = ParameterDirection.Input;
                        if (!isEdit)
                        {
                            cmd.Parameters.AddWithValue("@repID", repID);
                            cmd.Parameters["@repID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@compID", MainVM.SelectedCustomerSupplier.CompanyID);
                            cmd.Parameters["@compID"].Direction = ParameterDirection.Input;
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
                }
            }
            else if (employeeDetailsFormGridBg.IsVisible)
            {
                var dbCon = DBConnection.Instance();
                string[] proc = { "", "", "", "" };

                if (!isEdit)
                {
                    proc[0] = "INSERT_EMPLOYEE";
                }
                else
                {
                    proc[0] = "UPDATE_EMPLOYEE";
                }

                try
                {
                    using (MySqlConnection conn = dbCon.Connection)
                    {
                        conn.Open();
                        string empID = "";
                        MySqlCommand cmd = new MySqlCommand(proc[0], conn);
                        //INSERT NEW EMPLOYEE TO DB;
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

                        SecureString passwordsalt = empPasswordTb.SecurePassword;
                        foreach (Char c in "$w0rdf!$h")
                        {
                            passwordsalt.AppendChar(c);
                        }
                        passwordsalt.MakeReadOnly();

                        cmd.Parameters.AddWithValue("@upassword", SecureStringToString(passwordsalt));
                        cmd.Parameters["@upassword"].Direction = ParameterDirection.Input;

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
                        if (!isEdit)
                        {
                            cmd.Parameters.Add("@insertedid", MySqlDbType.Int32);
                            cmd.Parameters["@insertedid"].Direction = ParameterDirection.Output;
                            cmd.ExecuteNonQuery();
                            empID = cmd.Parameters["@insertedid"].Value.ToString();
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@empID", MainVM.SelectedEmployeeContractor.EmpID);
                            cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        if (!isEdit)
                        {
                            cmd = new MySqlCommand("INSERT_EMPLOYEE_PIC_T", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@picBLOB", picdata);
                            cmd.Parameters["@picBLOB"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@sigBLOB", null);
                            cmd.Parameters["@sigBLOB"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@empID", empID);
                            cmd.Parameters["@empID"].Direction = ParameterDirection.Input;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd = new MySqlCommand("UPDATE_EMPLOYEE_PIC_T", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@picBLOB", picdata);
                            cmd.Parameters["@picBLOB"].Direction = ParameterDirection.Input;
                            cmd.Parameters.AddWithValue("@sigBLOB", null);
                            cmd.Parameters["@sigBLOB"].Direction = ParameterDirection.Input;
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
                    loadDataToUi();
                }
            }
        }

        private void resetFieldsValue()
        {

            companyDetailsFormGridBg.Visibility = Visibility.Collapsed;
            employeeDetailsFormGridBg.Visibility = Visibility.Collapsed;
            foreach (var element in companyDetailsFormGrid.Children)
            {
                if (element is TextBox)
                {
                    ((TextBox)element).Text = string.Empty;
                }
                if (element is ComboBox)
                {
                    ((ComboBox)element).SelectedIndex = -1;
                }
            }
            foreach (var element in employeeDetailsFormGrid.Children)
            {
                if (element is TextBox)
                {
                    ((TextBox)element).Text = string.Empty;
                }
                if (element is ComboBox)
                {
                    ((ComboBox)element).SelectedIndex = -1;
                }
            }
        }

        //Settings Grid

        private static String dbname = "odc_db";

        //EMPLOYEE PART
        private void addEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addEmpPosBtn.Content.Equals("Save"))
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (String.IsNullOrWhiteSpace(empPosNewTb.Text))
                {
                    MessageBox.Show("Employee Position must be filled");
                }
                else
                {
                    if (employeePositionLb.Items.Contains(empPosNewTb.Text))
                    {
                        MessageBox.Show("Employee Position already exists");
                    }
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `odc_db`.`position_t` set `positionName` = '" + empPosNewTb.Text + "' where positionID = '" + MainMenu.MainVM.SelectedEmpPosition.PositionID + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Employee Poisition saved");
                            addEmpPosBtn.Content = "Add";
                            empPosNewTb.Clear();
                            loadDataToUi();
                            dbCon.Close();
                        }
                    }
                }
            }
            else
            {
                string strPosition = empPosNewTb.Text;
                if (String.IsNullOrWhiteSpace(empPosNewTb.Text))
                {
                    MessageBox.Show("Employee Position field must be filled");
                }
                else
                {
                    var dbCon = DBConnection.Instance();
                    dbCon.DatabaseName = dbname;
                    if (employeePositionLb.Items.Contains(empPosNewTb.Text))
                    {
                        MessageBox.Show("Employee position already exists");
                    }
                    if (dbCon.IsConnect())
                    {
                        if (!Regex.IsMatch(strPosition, @"[a-zA-Z -]"))
                        {
                            MessageBox.Show("Special characters are not accepted");
                            empPosNewTb.Clear();
                        }
                        else
                        {
                            string query = "INSERT INTO `odc_db`.`position_t` (`positionName`) VALUES('" + empPosNewTb.Text + "')";
                            if (dbCon.insertQuery(query, dbCon.Connection))
                            {
                                {
                                    MessageBox.Show("Employee Position successfully added");
                                    empPosNewTb.Clear();
                                    loadDataToUi();
                                    dbCon.Close();
                                }
                            }
                        }
                    }
                }
            }
            
        }


        private void deleteEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            if (employeePositionLb.SelectedItems.Count > 0)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (dbCon.IsConnect())
                {
                    try
                    {
                        string query = "DELETE FROM `odc_db`.`position_t` WHERE `positionID`='" + MainMenu.MainVM.SelectedEmpPosition.PositionID + "';";

                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            dbCon.Close();
                            MessageBox.Show("Employee position successfully deleted.");
                            loadDataToUi();
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            else
            {
                MessageBox.Show("Select an employee position first.");
            }

        }

        private void editEmpPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (dbCon.IsConnect())
            {
                if (employeePositionLb.SelectedItems.Count > 0)
                {
                    empPosNewTb.Text = MainMenu.MainVM.SelectedEmpPosition.PositionName;
                    addEmpPosBtn.Content = "Save";

                }
                else
                {
                    MessageBox.Show("Please select an employee position first.");
                }
            }
            dbCon.Close();
        }

        //CONTRACTOR PART
        private void addContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addContJobBtn.Content.Equals("Save"))
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (String.IsNullOrWhiteSpace(contNewJobTb.Text))
                {
                    MessageBox.Show("Contractor Job Title field must be filled");
                }
                else
                {
                    if (contJobLb.Items.Contains(contNewJobTb.Text))
                    {
                        MessageBox.Show("Job Title already exists");
                    }
                    if (dbCon.IsConnect())
                    {
                        string query = "UPDATE `odc_db`.`job_title_t` set `jobName` = '" + contNewJobTb.Text + "' where jobID = '" + MainMenu.MainVM.SelectedJobTitle.JobID + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Job Title successfully saved");
                            contNewJobTb.Clear();
                            loadDataToUi();
                            dbCon.Close();
                            contNewJobTb.Clear();
                            addContJobBtn.Content = "Add";


                        }
                    }
                }
            }
            else
            {
                string strJobTitle = contNewJobTb.Text;
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (String.IsNullOrWhiteSpace(contNewJobTb.Text))
                {
                    MessageBox.Show("Contractor Job Title field must be field");
                }
                else
                {
                    if (contJobLb.Items.Contains(contNewJobTb.Text))
                    {
                        MessageBox.Show("Contractor Job Title already exists");
                    }
                    else
                    {
                        if (!Regex.IsMatch(strJobTitle, @"[a-zA-Z -]"))
                        {
                            MessageBox.Show("Special Characters are not accepted");
                            contNewJobTb.Clear();
                        }
                        else
                        {
                            if (dbCon.IsConnect())
                            {
                                string query = "INSERT INTO `odc_db`.`job_title_t` (`jobName`) VALUES('" + contNewJobTb.Text + "')";
                                if (dbCon.insertQuery(query, dbCon.Connection))
                                {
                                    MessageBox.Show("Contractor Job Title successfully added");
                                    contNewJobTb.Clear();
                                    loadDataToUi();
                                    dbCon.Close();
                                }
                            }
                        }
                    }
                }
            }
        }


        private void deleteContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            if (contJobLb.SelectedItems.Count > 0)
            {
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;
                if (dbCon.IsConnect())
                {
                    try
                    {
                        string query = "DELETE FROM `odc_db`.`job_title_t` WHERE `JobID`='" + MainMenu.MainVM.SelectedJobTitle.JobID + "';";

                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            dbCon.Close();
                            MessageBox.Show("Job Position successfully deleted.");
                            loadDataToUi();
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            else
            {
                MessageBox.Show("Select a Job Position first.");
            }
        }

        private void editContJobBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;

            if (contJobLb.SelectedItems.Count > 0)
            {
                contNewJobTb.Text = MainMenu.MainVM.SelectedJobTitle.JobName;
            }
            else
            {
                MessageBox.Show("Please select a record first.");
            }
            dbCon.Close();
            addContJobBtn.Content = "Save";
        }
        

        //product category
        private void deleteCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (invProductsCategoryLb.SelectedItems.Count > 0)
            {
                if (dbCon.IsConnect())
                {
                    string query = "DELETE FROM `odc_db`.`item_type_t` WHERE `typeID`='" + invProductsCategoryLb.SelectedValue + "';";
                    if (dbCon.deleteQuery(query, dbCon.Connection))
                    {
                        dbCon.Close();
                        loadDataToUi();
                        MessageBox.Show("Product Category successfully deleted");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose a Product Category first.");
            }
        }

        private void addCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (addCategoryBtn.Content.Equals("Save"))
            {

            }
            else
            {
                string strCategory = invCategoryTb.Text;
                var dbCon = DBConnection.Instance();
                dbCon.DatabaseName = dbname;

                if (!String.IsNullOrWhiteSpace(invCategoryTb.Text))
                {
                    if (invProductsCategoryLb.Items.Contains(invCategoryTb.Text))
                    {
                        MessageBox.Show("Product Category already exists");
                    }
                    else
                    {
                        if (!Regex.IsMatch(strCategory, @"[a-zA-Z -]"))
                        {
                            MessageBox.Show("Special characters are not accepted");
                            invCategoryTb.Clear();
                        }
                        else
                        {
                            if (dbCon.IsConnect())
                            {
                                string query = "INSERT INTO `odc_db`.`item_type_t` (`typeName`) VALUES('" + invCategoryTb.Text + "')";
                                if (dbCon.insertQuery(query, dbCon.Connection))
                                {

                                    MessageBox.Show("Product Category successfully added");
                                    loadDataToUi();
                                    invCategoryTb.Clear();
                                    dbCon.Close();
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product Category field must be filled");
                }
            }
            
        }

        private void editCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (invProductsCategoryLb.SelectedItems.Count > 0)
            {
                invCategoryTb.Text = MainVM.SelectedProductCategory.TypeName;
                addCategoryBtn.Content = "Save";
            }
            else
            {
                MessageBox.Show("Please select a product category first.");
            }
            dbCon.Close();
        }
        
        //service types
        private void addServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            serviceTypeList.Visibility = Visibility.Collapsed;
            serviceTypeAdd.Visibility = Visibility.Visible;
        }

        private void cancelServiceTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            serviceTypeList.Visibility = Visibility.Visible;
            serviceTypeAdd.Visibility = Visibility.Collapsed;
            loadDataToUi();
        }

        private void serviceName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string strService = (sender as TextBox).Text;
            if (!Regex.IsMatch(strService, @"[a-zA-Z -]"))
            {
                MessageBox.Show("Special characters are not accepted");
            }
            //if (System.Windows.Controls.Validation.GetHasError(serviceName) == true)
            //  saveServiceTypeBtn.IsEnabled = false;
            else
            {
                validateTextBoxes();
            }

        }

        private void servicePrice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            validateTextBoxes();
        }

        private void validateTextBoxes()
        {
            if (serviceName.Text.Equals("") || servicePrice.Value == 0)
            {
                saveServiceTypeBtn.IsEnabled = false;
            }
            else
            {
                saveServiceTypeBtn.IsEnabled = true;
            }
        }
        private string id = "";
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
                        MessageBox.Show("Service type successfully added!");
                        serviceTypeList.Visibility = Visibility.Visible;
                        serviceTypeAdd.Visibility = Visibility.Collapsed;

                        //clearing textboxes
                        serviceName.Clear();
                        serviceDesc.Clear();
                        servicePrice.Value = 0;
                        loadDataToUi();
                    }
                }
                else
                {
                    string query = "UPDATE `service_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + id + "'";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        //MessageBox.Show("Sevice type sucessfully updated");
                        id = "";
                        serviceTypeList.Visibility = Visibility.Visible;
                        serviceTypeAdd.Visibility = Visibility.Collapsed;
                        loadDataToUi();
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
                loadDataToUi();
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }

        }

        private void btnEditService_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = dbname;
            if (serviceTypeDg.SelectedItems.Count > 0)
            {

                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
                serviceName.Text = MainVM.SelectedService.ServiceName;
                serviceDesc.Text = MainVM.SelectedService.ServiceDesc;
                servicePrice.Value = MainVM.SelectedService.ServicePrice;
                dbCon.Close();
                serviceTypeList.Visibility = Visibility.Collapsed;
                serviceTypeAdd.Visibility = Visibility.Visible;
            }

        }

        private void btnDeleteService_Click(object sender, RoutedEventArgs e)
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

        bool initPrice = true;
        string locationid = "";
        private void custProvinceCb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (provinceCb.SelectedIndex == -1)
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
                        "WHERE locationProvinceId = '" + provinceCb.SelectedValue + "';";
                    MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                    DataSet fromDb = new DataSet();
                    DataTable fromDbTable = new DataTable();
                    dataAdapter.Fill(fromDb, "t");
                    fromDbTable = fromDb.Tables["t"];
                    if (fromDbTable.Rows.Count != 0)
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
                        string query = "INSERT INTO location_details_t (locationProvinceID,locationPrice) VALUES ('" + provinceCb.SelectedValue + "', '" + locationPrice.Value + "')";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            MessageBox.Show("Price saved.");
                            provinceCb.SelectedValue = -1;
                            locationPrice.Value = 0;

                            serviceName.Clear();
                            serviceDesc.Clear();
                            servicePrice.Value = 0;
                            loadDataToUi();
                        }
                    }
                    else
                    {
                        string query = "UPDATE `location_details_t` SET locationPrice = '" + locationPrice.Value + "' WHERE locationId = '" + locationid + "'";
                        if (dbCon.insertQuery(query, dbCon.Connection))
                        {
                            //MessageBox.Show("Price updated.");
                            id = "";
                            provinceCb.SelectedValue = -1;
                            locationPrice.Value = 0;
                            initPrice = true;
                            loadDataToUi();
                        }
                    }



                }
                else if (result == MessageBoxResult.No)
                {
                    provinceCb.SelectedValue = -1;
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

        
    }
}
