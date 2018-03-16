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
        ObservableCollection<Employee> notAvail = new ObservableCollection<Employee>();
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

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                loadDataToUi();

            }
        }

        void loadDataToUi()
        {
            if (!MainVM.isNewSched)
            {
                MainVM.SelectedAvailedServices = MainVM.AvailedServices.Where(x => x.AvailedServiceID == MainVM.SelectedServiceSchedule_.ServiceAvailedID).FirstOrDefault();
                var dbCon = DBConnection.Instance();
                string query = "SELECT * FROM phases_per_services_t where serviceSchedID = " + MainVM.SelectedServiceSchedule_.ServiceSchedID;
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.SelectedServiceSchedule_.PhasesPerService.Add(new PhasesPerService() { ID = int.Parse(dr[0].ToString()), ServiceSchedID = int.Parse(dr[2].ToString()), PhaseID = int.Parse(dr[1].ToString()), Status = dr[3].ToString()});
                }
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

        void searchForAvailableEmployees()
        {
            MainVM.AvailableEmployees_.Clear();
            IEnumerable<Employee> availEmp;
            IEnumerable<Employee> availCont;
            
            foreach (ServiceSchedule ss in MainVM.ServiceSchedules_)
            {
                if (ss.dateEnded_ > startDate.SelectedDate)
                {
                    var empx = from emp in MainVM.Employees
                               join ssx in ss.assignedEmployees_ on emp.EmpID equals ssx.EmpID
                               select emp;
                    var contx = from cont in MainVM.Contractor
                                join ssx in ss.assignedEmployees_ on cont.EmpID equals ssx.EmpID
                                select cont;
                    foreach(Employee emp in empx)
                    {
                        notAvail.Add(emp);
                    }
                    foreach (Employee cont in contx)
                    {
                        notAvail.Add(cont);
                    }
                }
                    
            }

            foreach (Employee ee in MainVM.Employees)
            {
                if (!notAvail.Contains(ee))
                    MainVM.AvailableEmployees_.Add(ee);
            }
            foreach (Employee ee in MainVM.Contractor)
            {
                if (!notAvail.Contains(ee))
                    MainVM.AvailableEmployees_.Add(ee);
            }

            //if (MainVM.ServiceSchedules_.Count ==0)
            //{

            //    foreach (Employee ee in MainVM.Employees)
            //    {
            //        if(!MainVM.SelectedServiceSchedule_.assignedEmployees_.Contains(ee))
            //            MainVM.AvailableEmployees_.Add(ee);
            //    }
            //    foreach (Employee ee in MainVM.Contractor)
            //    {
            //        if (!MainVM.SelectedServiceSchedule_.assignedEmployees_.Contains(ee))
            //            MainVM.AvailableEmployees_.Add(ee);
            //    }
            //}

            
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

        private void saveSchedBtn_Click(object sender, RoutedEventArgs e)
        {
            saveDataToDb();
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
                if (!MainVM.isEdit)
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
                            query = "INSERT INTO `odc_db`.`phases_per_services_t`(`phaseID`,`serviceSchedID`, `status`)" +
                         " VALUES" +
                         "('"+ pps.PhaseID + "','" +
                            schedID + "','PENDING');";
                            dbCon.insertQuery(query, dbCon.Connection);
                        }

                        MessageBox.Show("Succesfully added a schedule");
                    }
                }
                else
                {
                    string query = "UPDATE `odc_db`.`service_sched_t` " +
                        "SET" +
                        " `dateStarted` = '" + startDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "','" +
                        " `dateEnded` = '" + endDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "','" +
                        " `schedNotes` =  '" + notesTb.Text + "','" +
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
                MainVM.isEdit = false;
            }
        }

        

        

        private void transferToLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.AvailableEmployees_.Add(MainVM.SelectedEmployeeContractor);
            MainVM.SelectedServiceSchedule_.assignedEmployees_.Remove(MainVM.SelectedEmployeeContractor);
            
        }

        private void transferToRightBtn_Click(object sender, RoutedEventArgs e)
        {
            if(notAvail.Contains(MainVM.SelectedEmployeeContractor) && MainVM.Employees.Contains(MainVM.SelectedEmployeeContractor))
            {
                MessageBoxResult result = MessageBox.Show("This employee already assigned to other service, do you want to assign this ", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
            MainVM.SelectedServiceSchedule_.assignedEmployees_.Add(MainVM.SelectedEmployeeContractor);
            MainVM.AvailableEmployees_.Remove(MainVM.SelectedEmployeeContractor);
        }

        private void startDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endDate.DisplayDateStart = startDate.SelectedDate;
            searchForAvailableEmployees();
        }

        private void endDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void markAsDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            using (MySqlConnection conn = dbCon.Connection)
            {
                string query = "UPDATE `odc_db`.`phases_per_services_t` SET `status` = 'DONE' WHERE id = '" + MainVM.SelectedPhasesPerService.ID + "'";
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

                
            }
        }
    }
}
