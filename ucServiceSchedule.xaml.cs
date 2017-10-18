using MySql.Data.MySqlClient;
using Syncfusion.UI.Xaml.Schedule;
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
            workerAtServiceSched.DoWork += workerAtServiceSched_DoWork;
            workerAtServiceSched.RunWorkerCompleted += workerAtServiceSched_RunWorkerCompleted;
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

        private void workerAtServiceSched_DoWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { searchForAvailableEmployees(); }));
        }

        private void workerAtServiceSched_RunWorkerCompleted(object sender,
                                               RunWorkerCompletedEventArgs e)
        {

        }

        private void scheduleServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Visible;
            MainVM.SelectedServiceSchedule_ = new ServiceSchedule();
        }
        private void Btn_ScheduleType_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "Day":
                    {
                        serviceSched.ScheduleType = ScheduleType.Day;
                        break;
                    }
                case "Week":
                    {
                        serviceSched.ScheduleType = ScheduleType.Week;
                        break;
                    }
                case "WorkWeek":
                    {
                        serviceSched.ScheduleType = ScheduleType.WorkWeek;
                        break;
                    }
                case "Month":
                    {
                        serviceSched.ScheduleType = ScheduleType.Month;
                        break;
                    }
                case "TimeLine":
                    {
                        serviceSched.ScheduleType = ScheduleType.TimeLine;
                        break;
                    }
            }
        }

        private void serviceSched_ScheduleClick(object sender, ScheduleClickEventArgs e)
        {

        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
        }

        private void saveSchedBtn_Click(object sender, RoutedEventArgs e)
        {
            if (assignedEmployees.Items.Count != 0)
            {
                serviceSched.Appointments.Add(new ScheduleAppointment() { Subject = serviceNoCb.SelectedValue.ToString(), StartTime = (DateTime)startDate.SelectedDate, EndTime = (DateTime)endDate.SelectedDate });
                MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ = serviceNoCb.SelectedValue.ToString();
                MainVM.SelectedServiceSchedule_.dateStarted_ = (DateTime)startDate.SelectedDate;
                MainVM.SelectedServiceSchedule_.dateEnded_ = (DateTime)endDate.SelectedDate;
                MainVM.SelectedServiceSchedule_.schedNotes_ = notesTb.Text;

                saveDataToDb();
                Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                formGridBg.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("No Assigned Employees");
            }
            
        }

        private void cancelschedBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
        }

        private void serviceSched_AppointmentEditorOpening(object sender, AppointmentEditorOpeningEventArgs e)
        {
            e.Cancel = true;
            if(e.Action == EditorAction.Add)
            {
                Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                formGridBg.Visibility = Visibility.Collapsed;
                MainVM.SelectedServiceSchedule_ = new ServiceSchedule();
            }
            else if(e.Action == EditorAction.Edit){

                Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                formGridBg.Visibility = Visibility.Collapsed;
                loadDataToUi();
            }
        }

        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            using (MySqlConnection conn = dbCon.Connection)
            {

                conn.Open();
                MySqlCommand cmd = null;
                if (!MainVM.isEdit)
                {
                    string query = "INSERT INTO `odc_db`.`service_sched_t`(`serviceSchedNoChar`,`invoiceNo`,`dateStarted`,`dateEnded`,`schedNotes`)" +
                        " VALUES" +
                        "('"
                        + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "','" +
                          MainVM.SelectedServiceSchedule_.invoiceNo_ + "','" +
                          MainVM.SelectedServiceSchedule_.dateStarted_.ToString("yyyy-MM-dd") + "','" +
                          MainVM.SelectedServiceSchedule_.dateEnded_.ToString("yyyy-MM-dd") + "','" +
                          MainVM.SelectedServiceSchedule_.schedNotes_ + "','" +
                        "');";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        foreach (AssignedEmployee emp in MainVM.SelectedServiceSchedule_.assignedEmployees_)
                        {
                           query = "INSERT INTO `odc_db`.`assigned_employees_t`(`serviceSqNoChar`,`empID`)" +
                        " VALUES" +
                        "('"
                        + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "','" +
                          emp.EmpID_ +
                        "');";
                            dbCon.insertQuery(query, dbCon.Connection);
                        }
                        MessageBox.Show("Succesfully added a schedule");
                    }
                }
                else
                {
                    string query = "UPDATE `odc_db`.`service_sched_t` " +
                        "SET" +
                        " `serviceSchedNoChar` = '" + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "','" +
                        " `invoiceNo` = '" + MainVM.SelectedServiceSchedule_.invoiceNo_ + "','" +
                        " `dateStarted` = '" + MainVM.SelectedServiceSchedule_.dateStarted_.ToString("yyyy-MM-dd") + "','" +
                        " `dateEnded` = '" + MainVM.SelectedServiceSchedule_.dateEnded_.ToString("yyyy-MM-dd") + "','" +
                        " `schedNotes` =  '" + MainVM.SelectedServiceSchedule_.schedNotes_ + "','" +
                        " WHERE `serviceSchedNoChar` = " + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ +
                        ";";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        foreach (AssignedEmployee emp in MainVM.SelectedServiceSchedule_.assignedEmployees_)
                        {
                            query = "UPDATE `odc_db`.`assigned_employees_t` SET " +
                                "`serviceSqNoChar` = '" + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "','" +
                                "`empID` =  '" + emp.EmpID_ + "' "+
                                "WHERE `assignEmployeeID` = '"+emp.TableID_+"';";
                            dbCon.insertQuery(query, dbCon.Connection);
                        }
                        MessageBox.Show("Succesfully updated the schedule");
                    }
                }
                MainVM.isEdit = false;
            }
        }

        void loadDataToUi()
        {
            MainVM.isEdit = true;
            MainVM.SelectedServiceSchedule_ = MainVM.ServiceSchedules_.Where(x => x.serviceSchedNoChar_.Equals(serviceSched.SelectedAppointment.Subject)).First();
        }

        void searchForAvailableEmployees()
        {
            IEnumerable<Employee> observable = null;
            foreach (AssignedEmployee ae in MainVM.AssignedEmployees_)
            {
                observable = MainVM.AllEmployeesContractor.Where(x => !(x.EmpID.Equals(ae.EmpID_)));
            }
            availableEmployees.ItemsSource = new ObservableCollection<Employee>(observable);
        }

        private void transferToLeftBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void transferToRightBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void startDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            workerAtServiceSched.RunWorkerAsync();
        }

        private void endDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            workerAtServiceSched.RunWorkerAsync();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
