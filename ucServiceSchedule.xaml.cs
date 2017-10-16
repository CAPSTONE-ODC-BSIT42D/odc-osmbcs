using Syncfusion.UI.Xaml.Schedule;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void scheduleServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["sbShowRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
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

        private void addEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void saveSchedBtn_Click(object sender, RoutedEventArgs e)
        {
            MainVM.ServiceSchedules_.Add(new ServiceSchedule() { serviceSchedNoChar_ = serviceNoCb.SelectedValue.ToString(), dateStarted_ = (DateTime)startDate.SelectedDate, dateEnded_ = (DateTime)endDate.SelectedDate });
            Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
            sb.Begin(formGridBg);
            formGridBg.Visibility = Visibility.Collapsed;
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
            }
            else if(e.Action == EditorAction.Edit){
                Storyboard sb = Resources["sbHideRightMenu"] as Storyboard;
                sb.Begin(formGridBg);
                formGridBg.Visibility = Visibility.Collapsed;
                loadDataToUi();
            }
        }

        void loadDataToUi()
        {
            MainVM.SelectedServiceSchedule_ = MainVM.ServiceSchedules_.Where(x => x.serviceSchedNoChar_.Equals(serviceSched.SelectedAppointment.Subject)).First();

        }
    }
}
