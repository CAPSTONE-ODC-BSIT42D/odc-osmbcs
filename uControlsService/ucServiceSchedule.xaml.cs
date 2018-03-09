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
        public static readonly BackgroundWorker workerAtServiceSched = new BackgroundWorker();

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
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
                if (MainVM.SelectedAvailedServices != null)
                {
                    loadDataToUi();
                    
                }
                    
            }
        }

        void loadDataToUi()
        {
            if (MainVM.isEdit)
            {
                var dbCon = DBConnection.Instance();
                string query = "SELECT * FROM phases_per_services where serviceSchedID = " + MainVM.SelectedServiceSchedule_.ServiceSchedID;
                MySqlDataAdapter dataAdapter = dbCon.selectQuery(query, dbCon.Connection);
                DataSet fromDb = new DataSet();
                DataTable fromDbTable = new DataTable();
                dataAdapter.Fill(fromDb, "t");
                fromDbTable = fromDb.Tables["t"];
                foreach (DataRow dr in fromDbTable.Rows)
                {
                    MainVM.SelectedServiceSchedule_.PhasesPerService.Add(new PhasesPerService() { ID = int.Parse(dr[0].ToString()), ServiceSchedID = int.Parse(dr[1].ToString()), PhaseID = int.Parse(dr[2].ToString()), Status = dr[3].ToString()});
                }
                startDate.SelectedDate = MainVM.SelectedServiceSchedule_.dateStarted_;
                endDate.SelectedDate = MainVM.SelectedServiceSchedule_.dateEnded_;
                notesTb.Text = MainVM.SelectedServiceSchedule_.schedNotes_;
            }
            else
            {
                MainVM.SelectedServiceSchedule_ = new ServiceSchedule();
                var obj = from ph in MainVM.Phases
                          join pg in MainVM.PhasesGroup on ph.PhaseGroupID equals pg.PhaseGroupID
                          where pg.ServiceID == MainVM.SelectedAvailedServices.ServiceID
                          select ph;
                foreach(Phase ph in obj)
                {
                    MainVM.SelectedServiceSchedule_.PhasesPerService.Add(new PhasesPerService() { PhaseID = ph.PhaseID, Status = "PENDING" });
                }
            }

            
        }

        void searchForAvailableEmployees()
        {
            foreach(ServiceSchedule ss in MainVM.ServiceSchedules_)
            {
                if(ss.dateEnded_ < startDate.SelectedDate)
                {
                    var emp = (from e in MainVM.Employees
                               where !(ss.assignedEmployees_.Contains(e))
                               select e);
                    var cont = (from e in MainVM.Contractor
                                where !(ss.assignedEmployees_.Contains(e))
                                select e);

                    foreach (Employee ee in emp)
                    {
                        MainVM.AvailableEmployees_.Add(ee);
                    }
                    foreach (Employee ee in cont)
                    {
                        MainVM.AvailableEmployees_.Add(ee);
                    }
                }
                
            }
            if(MainVM.ServiceSchedules_.Count ==0)
            {

                foreach (Employee ee in MainVM.Employees)
                {
                    MainVM.AvailableEmployees_.Add(ee);
                }
                foreach (Employee ee in MainVM.Contractor)
                {
                    MainVM.AvailableEmployees_.Add(ee);
                }
            }

            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void selectServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSelectServiceButtonClicked(e);
            loadDataToUi();
            searchForAvailableEmployees();
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
            ////MainVM.SelectedSalesQuote = MainVM.SalesQuotes.Where(x => x.sqNoChar_.Equals(MainVM.SelectedAddedService.SqNoChar)).First();
            //MainVM.SelectedSalesInvoice = MainVM.SalesInvoice.Where(x => x.sqNoChar_.Equals(MainVM.SelectedSalesQuote.sqNoChar_)).First();
            //if (assignedEmployees.Items.Count != 0 && MainVM.SelectedSalesInvoice!=null)
            //{
            //    //serviceSched.Appointments.Add(new ScheduleAppointment() { Subject = serviceNoCb.SelectedValue.ToString(), StartTime = (DateTime)startDate.SelectedDate, EndTime = (DateTime)endDate.SelectedDate });


            //    MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ = serviceNoCb.SelectedValue.ToString();
            //    MainVM.SelectedServiceSchedule_.invoiceNo_ = int.Parse(MainVM.SelectedSalesInvoice.invoiceNo_);
            //    MainVM.SelectedServiceSchedule_.dateStarted_ = (DateTime)startDate.SelectedDate;
            //    MainVM.SelectedServiceSchedule_.dateEnded_ = (DateTime)endDate.SelectedDate;
            //    MainVM.SelectedServiceSchedule_.schedNotes_ = notesTb.Text;
            //    saveDataToDb();
            //    Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            //    sb.Begin(formGridBg);
            //    formGridBg.Visibility = Visibility.Collapsed;
            //    MainVM.AssignedEmployees_.Clear();
            //}
            //else
            //{
            //    MessageBox.Show("No Assigned Employees");
            //}

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
                        foreach (Employee emp in MainVM.AssignedEmployees_)
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

        }
    }
}
