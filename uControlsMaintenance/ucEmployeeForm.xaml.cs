using System;
using System.Collections.Generic;
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

namespace prototype2.uControlsMaintenance
{
    /// <summary>
    /// Interaction logic for ucEmployeeForm.xaml
    /// </summary>
    public partial class ucEmployeeForm : UserControl
    {
        public ucEmployeeForm()
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

        private void cancelRecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to cancel?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
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
                foreach (var element in employeeForm.Children)
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
                    else if(element is StackPanel)
                    {
                        foreach(var element1 in perTypeField.Children)
                        {
                            if (((Grid)element1).Equals(employeeOnlyGrid))
                            {
                                foreach(var element1a in employeeOnlyGrid.Children)
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
                            }
                            else
                            {
                                foreach (var element1a in contractorOnlyGrid.Children)
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
                                    else if(element is DatePicker)
                                    {
                                        BindingExpression expression = ((DatePicker)element).GetBindingExpression(DatePicker.SelectedDateProperty);
                                        expression.UpdateSource();
                                        if (Validation.GetHasError((DatePicker)element))
                                            validationError = true;
                                    }
                                }
                            }
                        }
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

        private void hasAccessCb_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var element in accountCredentialsForm.Children)
            {
                if (element is Label)
                    ((Label)element).IsEnabled = true;
                else if (element is TextBox)
                    ((TextBox)element).IsEnabled = true;
            }
        }

        private void hasAccessCb_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var element in accountCredentialsForm.Children)
            {
                if (element is Label)
                    ((Label)element).IsEnabled = false;
                else if (element is TextBox)
                    ((TextBox)element).IsEnabled = false;
            }
            
        }

        private void addPosBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            if (newPosTb.IsVisible)
            {
                newPosTb.Visibility = Visibility.Collapsed;
                addPosBtn.Content = "+";
                string query = "INSERT INTO `odc_db`.`position_t` (`positionName`) VALUES('" + newPosTb.Text + "')";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    {
                        MessageBox.Show("Employee Position successfully added");
                        newPosTb.Clear();
                        MainVM.Ldt.worker.RunWorkerAsync();
                        dbCon.Close();
                    }
                }
            }
            else
            {
                newPosTb.Visibility = Visibility.Visible;
                addPosBtn.Content = "Save";
            }
                

        }

        private void addJobBtn_Click(object sender, RoutedEventArgs e)
        {

            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            if (newPosTb.IsVisible)
            {
                newJobTb.Visibility = Visibility.Collapsed;
                addJobBtn.Content = "+";
                string query = "INSERT INTO `odc_db`.`job_title_t` (`jobName`) VALUES('" + newJobTb.Text + "')";
                if (dbCon.insertQuery(query, dbCon.Connection))
                {
                    {
                        MessageBox.Show("Successfully added");
                        newJobTb.Clear();
                        MainVM.Ldt.worker.RunWorkerAsync();
                        dbCon.Close();
                    }
                }
            }
            else
            {
                newJobTb.Visibility = Visibility.Visible;
                addJobBtn.Content = "Save";
            }
            
        }

        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            if (MainVM.isEdit)
            {
                if (dbCon.IsConnect())
                {
                    string query = "";
                    if (!MainVM.isContractor)
                    {
                        query = "UPDATE `odc_db`.`emp_cont_t`" +
                        "SET `empFName` = '" + empFirstNameTb.Text + "'," +
                        "`empLName` = '" + empLastNameTb.Text + "'," +
                        "`empMI` = '" + empMiddleInitialTb.Text + "'," +
                        "`positionID` = '" + empPostionCb.SelectedValue + "'," +
                        "`empUserName` = '" + empUserNameTb.Text + "'," +
                        " `hasAccess` = " + (bool)hasAccessCb.IsChecked +
                        ") " +
                        " WHERE empID = " + MainVM.SelectedEmployeeContractor.EmpID +
                        ";";
                    }
                    else
                    {
                        query = "UPDATE `odc_db`.`emp_cont_t`" +
                        "SET `empFName` = '" + empFirstNameTb.Text + "'," +
                        "`empLName` = '" + empLastNameTb.Text + "'," +
                        "`empMI` = '" + empMiddleInitialTb.Text + "'," +
                        "`jobID` = '" + empJobCb.SelectedValue + "'," +
                        "`empAddress` = '" + empAddressTb.Text + "'," +
                        "`empDateFrom` = '" + empDateStarted.SelectedDate + "'," +
                        "`empDateTo` = '" + empDateEnded.SelectedDate +
                        "') " +
                        " WHERE empID = " + MainVM.SelectedEmployeeContractor.EmpID +
                        ";";
                    }

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Record is Saved");

                    }
                }
            }
            else
            {
                if (dbCon.IsConnect())
                {
                    string query = "";
                    if (!MainVM.isContractor)
                    {
                        query = "INSERT INTO `odc_db`.`emp_cont_t`   (`empFName`,`empLName`,`empMI`,`positionID`,`empUserName`,`empPassword`,`empDateFrom`,`empDateTo`,`empType`, `hasAccess`) " +
                        " VALUES " +
                        "('"
                        + empFirstNameTb.Text + "','" +
                        empLastNameTb.Text + "','" +
                        empMiddleInitialTb.Text + "','" +
                        empPostionCb.SelectedValue + "','" +
                        empUserNameTb.Text + "'," +
                        "md5('" +
                        empLastNameTb.Text +
                        "'),'" +
                        empDateStarted.SelectedDate + "','" +
                        empDateEnded.SelectedDate + "','0'," +
                        hasAccessCb.IsChecked +
                        "); ";
                    }
                    else
                    {
                        query = "INSERT INTO `odc_db`.`emp_cont_t`   (`empFName`,`empLName`,`empMI`,`empAddress`,`jobID`,`empDateFrom`,`empDateTo`,`empType`, `hasAccess`) " +
                        " VALUES " +
                        "('"
                        + empFirstNameTb.Text + "','" +
                        empLastNameTb.Text + "','" +
                        empMiddleInitialTb.Text + "','" +
                        empAddressTb.Text + "','" +
                        empJobCb.SelectedValue + "','" +
                        empDateStarted.SelectedDate + "','" +
                        empDateEnded.SelectedDate + "','1'," +
                        hasAccessCb.IsChecked +
                        "); ";
                    }

                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Record is Saved");

                    }
                }
            }
            
        }

        void resetFieldsValue()
        {
        }

        void loadDataToUi()
        {
            empFirstNameTb.Text = MainVM.SelectedEmployeeContractor.EmpFname;
            empLastNameTb.Text = MainVM.SelectedEmployeeContractor.EmpLName;
            empMiddleInitialTb.Text = MainVM.SelectedEmployeeContractor.EmpMiddleInitial;
            hasAccessCb.IsChecked = MainVM.SelectedEmployeeContractor.HasAccess;
            if (MainVM.SelectedEmployeeContractor.EmpType == 0)
            {
                contractorOnlyGrid.Visibility = Visibility.Collapsed;
                employeeOnlyGrid.Visibility = Visibility.Visible;
                formHeader.Content = "Employee Details";
                empPostionCb.SelectedValue = MainVM.SelectedEmployeeContractor.PositionID;
                empUserNameTb.Text = MainVM.SelectedEmployeeContractor.EmpUserName;
            }
            else if (MainVM.SelectedEmployeeContractor.EmpType == 1 && MainVM.isContractor)
            {
                contractorOnlyGrid.Visibility = Visibility.Visible;
                employeeOnlyGrid.Visibility = Visibility.Collapsed;
                formHeader.Content = "Employee Details";
                empAddressTb.Text = MainVM.SelectedEmployeeContractor.EmpAddress;
                empJobCb.SelectedValue = MainVM.SelectedEmployeeContractor.JobID;
                empDateStarted.SelectedDate = MainVM.SelectedEmployeeContractor.EmpDateTo;
                empDateEnded.SelectedDate = MainVM.SelectedEmployeeContractor.EmpDateFrom;
            }
        }

        private void uControlEmployeeForm_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (MainVM.isEdit && this.IsVisible)
            {
                loadDataToUi();
            }
            else
            {
                if (!MainVM.isContractor)
                {
                    contractorOnlyGrid.Visibility = Visibility.Collapsed;
                    employeeOnlyGrid.Visibility = Visibility.Visible;
                    formHeader.Content = "Employee Details";
                }
                else
                {
                    contractorOnlyGrid.Visibility = Visibility.Visible;
                    employeeOnlyGrid.Visibility = Visibility.Collapsed;
                    formHeader.Content = "Contractor Details";
                }
            }
        }
    }
}
