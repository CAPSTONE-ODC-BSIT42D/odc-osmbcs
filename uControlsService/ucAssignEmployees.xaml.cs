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

namespace prototype2.uControlsService
{
    /// <summary>
    /// Interaction logic for ucAssignEmployees.xaml
    /// </summary>
    public partial class ucAssignEmployees : UserControl
    {
        public ucAssignEmployees()
        {
            InitializeComponent();
        }
        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;

        public event EventHandler SaveCloseOtherButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseOtherButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private void closeModalBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void assignEmployee_Click(object sender, RoutedEventArgs e)
        {
            //if (MainVM.NotAvail.Contains(MainVM.SelectedEmployeeContractor) && MainVM.Employees.Contains(MainVM.SelectedEmployeeContractor))
            //{
            //    MessageBoxResult result = MessageBox.Show("This employee already assigned to other service, do you want to assign this ", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            //}
            MainVM.SelectedServiceSchedule_.assignedEmployees_.Add(MainVM.SelectedEmployeeContractor);
            MainVM.AvailableEmployees_.Remove(MainVM.SelectedEmployeeContractor);
        }


    }

}
