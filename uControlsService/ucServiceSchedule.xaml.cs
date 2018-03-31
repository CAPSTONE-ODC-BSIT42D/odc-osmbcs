using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace prototype2
{
    /// <summary>
    /// Interaction logic for ucServiceSchedule.xaml
    /// </summary>
    public partial class ucServiceSchedule : UserControl
    {
        public ucServiceSchedule()
        {
            InitializeComponent();
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;


        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            MainVM.resetValueofVariables();
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler SelectServiceButtonClicked;
        protected virtual void OnSelectServiceButtonClicked(RoutedEventArgs e)
        {
            var handler = SelectServiceButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler AssignEmployeeButtonClicked;
        protected virtual void OnAssignEmployeeButtonClicked(RoutedEventArgs e)
        {
            var handler = AssignEmployeeButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                searchForAvailableEmployees();
                loadDataToUi();
                if (MainVM.isViewSchedule)
                {
                    saveSchedBtn.Visibility = Visibility.Collapsed;
                    cancelschedBtn.Content = "Close";
                }
                else
                {
                    saveSchedBtn.Visibility = Visibility.Visible;
                    cancelschedBtn.Content = "Cancel";
                }

            }
        }

        void loadDataToUi()
        {
            if (!MainVM.isNewSchedule)
            {
                loadPhases();
                startDate.SelectedDate = MainVM.SelectedServiceSchedule_.dateStarted_;
                endDate.SelectedDate = MainVM.SelectedServiceSchedule_.dateEnded_;
                notesTb.Text = MainVM.SelectedServiceSchedule_.schedNotes_;
            }
            else if(MainVM.SelectedAvailedServices !=null)
            {
                MainVM.SelectedServiceSchedule_ = new ServiceSchedule();
                var obj = from ph in MainVM.Phases
                          where ph.ServiceID == MainVM.SelectedAvailedServices.ServiceID
                          select ph;
                foreach(Phase ph in obj)
                {
                    MainVM.SelectedServiceSchedule_.PhasesPerService.Add(new PhasesPerService() { PhaseID = ph.PhaseID, Status = "" });
                }
                
            }
        }

        void loadPhases()
        {
            
            MainVM.SelectedAvailedServices = MainVM.AvailedServices.Where(x => x.AvailedServiceID == MainVM.SelectedServiceSchedule_.ServiceAvailedID).FirstOrDefault();
            var dbCon = DBConnection.Instance();
            string query = "SELECT * FROM phases_per_services_t where serviceSchedID = " + MainVM.SelectedServiceSchedule_.ServiceSchedID;
            MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
            DataSet fromDb = new DataSet();
            DataTable fromDbTable = new DataTable();
            dataAdapter.Fill(fromDb, "t");
            fromDbTable = fromDb.Tables["t"];

            MainVM.SelectedServiceSchedule_.PhasesPerService.Clear();
            foreach (DataRow dr in fromDbTable.Rows)
            {
                DateTime dateStarted = new DateTime();
                DateTime.TryParse(dr[3].ToString(), out dateStarted);
                DateTime dateEnded = new DateTime();
                DateTime.TryParse(dr[4].ToString(), out dateEnded);

                MainVM.SelectedServiceSchedule_.PhasesPerService.Add(new PhasesPerService() { ID = int.Parse(dr[0].ToString()), ServiceSchedID = int.Parse(dr[2].ToString()), PhaseID = int.Parse(dr[1].ToString()), DateStarted = dateStarted, DateEnded = dateEnded, Status = dr[5].ToString() });
            }
        }

        void searchForAvailableEmployees()
        {
            MainVM.NotAvail.Clear();
            MainVM.AvailableEmployees_.Clear();
            //IEnumerable<Employee> availEmp;
            //IEnumerable<Employee> availCont;
            
            
            //foreach (ServiceSchedule ss in MainVM.ServiceSchedules_)
            //{
            //    if (ss.dateEnded_ > startDate.SelectedDate)
            //    {
            //        var empx = from emp in MainVM.Employees
            //                   join ssx in ss.assignedEmployees_ on emp.EmpID equals ssx.EmpID
            //                   select emp;
            //        var contx = from cont in MainVM.Contractor
            //                    join ssx in ss.assignedEmployees_ on cont.EmpID equals ssx.EmpID
            //                    select cont;
            //        foreach(Employee emp in empx)
            //        {
            //            MainVM.NotAvail.Add(emp);
            //        }
            //        foreach (Employee cont in contx)
            //        {
            //            MainVM.NotAvail.Add(cont);
            //        }
            //    }
                    
            //}

            foreach (Employee ee in MainVM.Employees)
            {
                if (!MainVM.SelectedServiceSchedule_.assignedEmployees_.Contains(ee))
                    MainVM.AvailableEmployees_.Add(ee);
            }
            foreach (Employee ee in MainVM.Contractor)
            {
                if (!MainVM.SelectedServiceSchedule_.assignedEmployees_.Contains(ee))
                    MainVM.AvailableEmployees_.Add(ee);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void selectServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSelectServiceButtonClicked(e);
            
        }

        private void scheduleServiceBtn_Click(object sender, RoutedEventArgs e)
        {
        }
        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
        }
        private bool validationError = false;
        private void saveSchedBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                foreach (var element in additionalFeesFormGrid1.Children)
                {
                    if (element is DatePicker)
                    {
                        BindingExpression expression = ((DatePicker)element).GetBindingExpression(DatePicker.SelectedDateProperty);
                        if (expression != null)
                        {
                            expression.UpdateSource();
                            if (Validation.GetHasError((DatePicker)element))
                                validationError = true;
                        }
                        if (((DatePicker)element).Equals(startDate) && ((DatePicker)element).SelectedDate < DateTime.Now)
                        {
                            BindingExpression bindingExpression = BindingOperations.GetBindingExpression(((DatePicker)element), DatePicker.SelectedDateProperty);

                            BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(((DatePicker)element), DatePicker.SelectedDateProperty);

                            ValidationError validationErrorA = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                            validationErrorA.ErrorContent = "Selected Date is Invalid. No past start date.";
                            Validation.MarkInvalid(bindingExpressionBase, validationErrorA);
                            validationError = true;
                        }
                    }
                    else if (element is DataGrid)
                    {
                        if (((DataGrid)element).Equals(assignedEmployees))
                        {
                            var hasEmployee = MainVM.SelectedServiceSchedule_.assignedEmployees_.Where(x => x.EmpType == 0);
                            
                            if (MainVM.SelectedServiceSchedule_.assignedEmployees_.Count == 0)
                            {
                                BindingExpression bindingExpression = BindingOperations.GetBindingExpression(((DataGrid)element), DataGrid.ItemsSourceProperty);

                                BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(((DataGrid)element), DataGrid.ItemsSourceProperty);

                                ValidationError validationErrorA = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                                validationErrorA.ErrorContent = "No assigned Employees";
                                Validation.MarkInvalid(bindingExpressionBase, validationErrorA);
                                validationError = true;
                            }
                            else if (hasEmployee.Count() == 0)
                            {
                                BindingExpression bindingExpression = BindingOperations.GetBindingExpression(((DataGrid)element), DataGrid.ItemsSourceProperty);

                                BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(((DataGrid)element), DataGrid.ItemsSourceProperty);

                                ValidationError validationErrorA = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                                validationErrorA.ErrorContent = "No assigned regular Employees";
                                Validation.MarkInvalid(bindingExpressionBase, validationErrorA);
                                validationError = true;
                            }
                        }
                        else
                        {

                        }
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

        private void cancelschedBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            using (MySqlConnection conn = dbCon.Connection)
            {
                if (!MainVM.isEditSchedule)
                {
                    string query = "INSERT INTO `odc_db`.`service_sched_t`(`serviceAvailedID`,`serviceStatus`,`dateStarted`,`dateEnded`,`schedNotes`)" +
                        " VALUES" +
                        "('"+
                          MainVM.SelectedAvailedServices.AvailedServiceID + "','PENDING','" +
                          startDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "','" +
                          endDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "','" +
                          notesTb.Text +
                        "');";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "SELECT LAST_INSERT_ID();";
                        string schedID = dbCon.selectScalar(query, dbCon.Connection).ToString();
                        foreach (Employee emp in MainVM.SelectedServiceSchedule_.assignedEmployees_)
                        {
                            query = "INSERT INTO `odc_db`.`assigned_employees_t`(`serviceSchedID`,`empID`)" +
                         " VALUES" +
                         "('" + schedID + "','" +
                           emp.EmpID +
                         "');";
                            dbCon.insertQuery(query, dbCon.Connection);
                        }

                        foreach(PhasesPerService pps in MainVM.SelectedServiceSchedule_.PhasesPerService)
                        {
                            if(MainVM.SelectedServiceSchedule_.PhasesPerService.IndexOf(pps) == 0)
                            {
                                query = "INSERT INTO `odc_db`.`phases_per_services_t`(`phaseID`,`serviceSchedID`,`dateStarted`)" +
                                        " VALUES" +
                                        "('" + pps.PhaseID + "','" +
                                            schedID + "','" +
                                            startDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "');";
                                dbCon.insertQuery(query, dbCon.Connection);
                            }
                            else
                            {
                                query = "INSERT INTO `odc_db`.`phases_per_services_t`(`phaseID`,`serviceSchedID`)" +
                                        " VALUES" +
                                        "('" + pps.PhaseID + "','" +
                                            schedID + "');";
                                dbCon.insertQuery(query, dbCon.Connection);
                            }

                        }

                        MessageBox.Show("Succesfully added a schedule");
                    }
                }
                else
                {
                    string query = "UPDATE `odc_db`.`service_sched_t` " +
                        "SET" +
                        " `dateStarted` = '" + startDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'," +
                        " `dateEnded` = '" + endDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'," +
                        " `schedNotes` =  '" + notesTb.Text + "'" +
                        " WHERE `serviceSchedID` = " + MainVM.SelectedServiceSchedule_.ServiceSchedID +
                        ";";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        query = "DELETE FROM `odc_db`.`assigned_employees_t` " +
                                "WHERE `serviceSchedID` = '" + MainVM.SelectedServiceSchedule_.ServiceSchedID + "';";
                        dbCon.insertQuery(query, dbCon.Connection);
                        foreach (Employee emp in MainVM.SelectedServiceSchedule_.assignedEmployees_)
                        {
                            query = "INSERT INTO `odc_db`.`assigned_employees_t`(`serviceSchedID`,`empID`)" +
                         " VALUES" +
                         "('"
                         + MainVM.SelectedServiceSchedule_.ServiceSchedID + "','" +
                           emp.EmpID +
                         "');";
                            dbCon.insertQuery(query, dbCon.Connection);
                        }
                        MessageBox.Show("Succesfully updated the schedule");
                    }

                }
            }
        }

        private void startDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsVisible)
            {

                endDate.DisplayDateStart = startDate.SelectedDate;
                if(MainVM.SelectedServiceSchedule_ != null)
                {
                    if (MainVM.SelectedServiceSchedule_.serviceStatus_.Equals("ON GOING") && MainVM.SelectedServiceSchedule_.dateStarted_ < startDate.SelectedDate)
                    {
                        var dbCon = DBConnection.Instance();
                        MessageBoxResult result = MessageBox.Show("Changing the start date will reset the schedule, Do you want to continue?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        if (result == MessageBoxResult.OK)
                        {
                            string query = "UPDATE `odc_db`.`phases_per_services_t` SET `status` = 'ON QUEUE', dateStarted = " + DBNull.Value + ", dateEnded = " + DBNull.Value + " WHERE serviceSchedID = '" + MainVM.SelectedServiceSchedule_.ServiceSchedID + "'";
                            dbCon.insertQuery(query, dbCon.Connection);
                        }
                        else if (result == MessageBoxResult.Cancel)
                        {
                            startDate.SelectedDate = MainVM.SelectedServiceSchedule_.dateStarted_;
                        }
                    }
                }

            }
        }

        private void endDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void markAsDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            using (MySqlConnection conn = dbCon.Connection)
            {
                string query = "UPDATE `odc_db`.`phases_per_services_t` SET `status` = 'DONE', dateEnded = '"+DateTime.Now.ToString("yyyy-MM-dd") +"'WHERE id = '" + MainVM.SelectedPhasesPerService.ID + "'";
                dbCon.insertQuery(query, dbCon.Connection);

                if (MainVM.SelectedServiceSchedule_.serviceStatus_.Equals("PENDING"))
                {
                    query = "UPDATE `odc_db`.`service_sched_t` SET serviceStatus = 'ON GOING' WHERE `serviceSchedID` = " + MainVM.SelectedServiceSchedule_.ServiceSchedID + ";";
                    dbCon.insertQuery(query, dbCon.Connection);
                }

                if(MainVM.SelectedServiceSchedule_.PhasesPerService.IndexOf(MainVM.SelectedPhasesPerService) != MainVM.SelectedServiceSchedule_.PhasesPerService.Count - 1)
                    MainVM.SelectedPhasesPerService = MainVM.SelectedServiceSchedule_.PhasesPerService[MainVM.SelectedServiceSchedule_.PhasesPerService.IndexOf(MainVM.SelectedPhasesPerService) + 1];
                query = "UPDATE `odc_db`.`phases_per_services_t` SET `dateStarted` = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'WHERE id = '" + MainVM.SelectedPhasesPerService.ID + "'";
                dbCon.insertQuery(query, dbCon.Connection);

                query = "SELECT * FROM PHASES_PER_SERVICES_T WHERE serviceSchedID = '"+ MainVM.SelectedServiceSchedule_.ServiceSchedID+ "' AND status = 'ON QUEUE'";
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                if(fromDbTable.Rows.Count == 0 )
                {
                    query = "UPDATE `odc_db`.`service_sched_t` SET `dateEnded` = '" + DateTime.Now.ToString("yyyy-MM-dd") + "', serviceStatus = 'DONE' WHERE `serviceSchedID` = " + MainVM.SelectedServiceSchedule_.ServiceSchedID + ";";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        MessageBox.Show("Succesfully updated the schedule");
                    }
                }
                
                else
                    MessageBox.Show("Succesfully updated the schedule");

            }
            loadPhases();
        }

        private void deletePhaseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedServiceSchedule_.PhasesPerService.Count > 1)
                MainVM.SelectedServiceSchedule_.PhasesPerService.Remove(MainVM.SelectedPhasesPerService);
            else
                MessageBox.Show("Atleast one phase is needed");
        }

        private void deleteAssignedEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedServiceSchedule_.assignedEmployees_.Count > 1)
            {
                var hasEmployee = MainVM.SelectedServiceSchedule_.assignedEmployees_.Where(x => x.EmpType == 0);
                if (hasEmployee.Count() > 0)
                {
                    MainVM.AvailableEmployees_.Add(MainVM.SelectedEmployeeContractor);
                    MainVM.SelectedServiceSchedule_.assignedEmployees_.Remove(MainVM.SelectedEmployeeContractor);
                }
                else
                    MessageBox.Show("Atleast one assigned regular employee needed");
            }
            else
                MessageBox.Show("Atleast one assigned employee needed");

        }

        private void assignEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAssignEmployeeButtonClicked(e);
        }
    }
}
