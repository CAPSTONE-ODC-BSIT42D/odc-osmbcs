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
            MainVM.ServiceSchedules_.Add(new ServiceSchedule() { serviceSchedNoChar_ = "sdadasdasdasdas", dateStarted_ = DateTime.Now, dateEnded_ = DateTime.Now.AddDays(10) });
            MainVM.ServiceSchedules_.Add(new ServiceSchedule() { serviceSchedNoChar_ = "112321", dateStarted_ = DateTime.Now.AddDays(10), dateEnded_ = DateTime.Now.AddDays(20) });
            MainVM.ServiceSchedules_.Add(new ServiceSchedule() { serviceSchedNoChar_ = "sdasdadas423adasdasdasdas", dateStarted_ = DateTime.Now.AddDays(20), dateEnded_ = DateTime.Now.AddDays(30) });
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
            MessageBox.Show("" + serviceSched.SelectedAppointment.Subject);
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

        }

        private void cancelschedBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
