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

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach(ServiceSchedule ss in MainVM.ServiceSchedules_)
            //serviceSched.Appointments.Add(new ScheduleAppointment() { Subject = ss.serviceSchedNoChar_, StartTime = (DateTime)ss.dateStarted_, EndTime = (DateTime)ss.dateEnded_ });
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
        }

        void saveDataToDb()
        {
            var dbCon = DBConnection.Instance();
            using (MySqlConnection conn = dbCon.Connection)
            {
                if (!MainVM.isEdit)
                {
                    string query = "INSERT INTO `odc_db`.`service_sched_t`(`serviceSchedNoChar`,`invoiceNo`,`dateStarted`,`dateEnded`,`schedNotes`)" +
                        " VALUES" +
                        "('"
                        + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "','" +
                          MainVM.SelectedServiceSchedule_.invoiceNo_ + "','" +
                          MainVM.SelectedServiceSchedule_.dateStarted_.ToString("yyyy-MM-dd") + "','" +
                          MainVM.SelectedServiceSchedule_.dateEnded_.ToString("yyyy-MM-dd") + "','" +
                          MainVM.SelectedServiceSchedule_.schedNotes_ +
                        "');";
                    if (dbCon.insertQuery(query, dbCon.Connection))
                    {
                        foreach (Employee emp in MainVM.AssignedEmployees_)
                        {
                            query = "INSERT INTO `odc_db`.`assigned_employees_t`(`serviceSqNoChar`,`empID`)" +
                         " VALUES" +
                         "('"
                         + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "','" +
                           emp.EmpID +
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
                        query = "DELETE FROM `odc_db`.`assigned_employees_t` " +
                                "WHERE `serviceSqNoChar` = '" + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "';";
                        dbCon.insertQuery(query, dbCon.Connection);
                        foreach (Employee emp in MainVM.SelectedServiceSchedule_.assignedEmployees_)
                        {
                            query = "INSERT INTO `odc_db`.`assigned_employees_t`(`serviceSqNoChar`,`empID`)" +
                             " VALUES" +
                             "('"
                             + MainVM.SelectedServiceSchedule_.serviceSchedNoChar_ + "','" +
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

        void loadDataToUi()
        {
            MainVM.isEdit = true;
            //MainVM.SelectedServiceSchedule_ = MainVM.ServiceSchedules_.Where(x => x.serviceSchedNoChar_.Equals(serviceSched.SelectedAppointment.Subject)).First();
        }

        void searchForAvailableEmployees()
        {

        }

        private void transferToLeftBtn_Click(object sender, RoutedEventArgs e)
        {

            MainVM.AssignedEmployees_.Add(MainVM.SelectedEmployeeContractor);
            MainVM.AvailableEmployees_.Remove(MainVM.SelectedEmployeeContractor);
        }

        private void transferToRightBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.AvailableEmployees_.Add(MainVM.SelectedEmployeeContractor);
            MainVM.AssignedEmployees_.Remove(MainVM.SelectedEmployeeContractor);
        }

        private void startDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            endDate.DisplayDateStart = startDate.SelectedDate;
            workerAtServiceSched.RunWorkerAsync();
        }

        private void endDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void serviceNoCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                //MainVM.SelectedSalesQuote = MainVM.SalesQuotes.Where(x => x.sqNoChar_.Equals(MainVM.SelectedAddedService.SqNoChar)).FirstOrDefault();
                MainVM.SelectedSalesInvoice = MainVM.SalesInvoice.Where(x => x.sqNoChar_.Equals(MainVM.SelectedSalesQuote.sqNoChar_)).FirstOrDefault();
                if (MainVM.SelectedSalesInvoice != null)
                {
                    // MainVM.SelectedService = MainVM.ServicesList.Where(x => x.ServiceID.Equals(MainVM.SelectedAddedService.ServiceID)).FirstOrDefault();
                }
                else
                {
                    MessageBox.Show("This service have no invoice");
                }
            }

        }
    }
}
