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
    /// Interaction logic for ucAddPhase.xaml
    /// </summary>
    public partial class ucAddPhase : UserControl
    {
        public ucAddPhase()
        {
            InitializeComponent();
        }

        public event EventHandler SaveCloseButtonClicked;
        protected virtual void OnSaveCloseButtonClicked(RoutedEventArgs e)
        {
            var handler = SaveCloseButtonClicked;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler AddPhaseItem;
        protected virtual void OnAddPhaseItem(RoutedEventArgs e)
        {
            var handler = AddPhaseItem;
            if (handler != null)
                handler(this, e);
        }

        MainViewModel MainVM = Application.Current.Resources["MainVM"] as MainViewModel;
        private bool validationError;

        private void addPhaseItemBtn_Click(object sender, RoutedEventArgs e)
        {
            OnAddPhaseItem(e);
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "odc_db";
            MessageBoxResult result = MessageBox.Show("Do you want to save this service type?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                //foreach (var element in serviceDetailsGrid.Children)
                //{
                //    if (element is TextBox)
                //    {
                //        BindingExpression expression = ((TextBox)element).GetBindingExpression(TextBox.TextProperty);
                //        if (expression != null)
                //        {
                //            if (((TextBox)element).IsEnabled)
                //            {
                //                expression.UpdateSource();
                //                if (Validation.GetHasError((TextBox)element))
                //                    validationError = true;
                //            }
                //        }
                //    }
                //    if (element is Xceed.Wpf.Toolkit.DecimalUpDown)
                //    {
                //        BindingExpression expression = ((Xceed.Wpf.Toolkit.DecimalUpDown)element).GetBindingExpression(Xceed.Wpf.Toolkit.DecimalUpDown.TextProperty);
                //        if (((Xceed.Wpf.Toolkit.DecimalUpDown)element).IsEnabled)
                //        {
                //            expression.UpdateSource();
                //            if (Validation.GetHasError((Xceed.Wpf.Toolkit.DecimalUpDown)element))
                //                validationError = true;
                //        }
                //    }
                //}
                //if (!validationError)
                //{
                //    if (!MainVM.isEdit)
                //    {
                //        string query = "INSERT INTO services_t (serviceName,serviceDesc,servicePrice) VALUES ('" + serviceName.Text + "','" + serviceDesc.Text + "', '" + servicePrice.Value + "')";
                //        if (dbCon.insertQuery(query, dbCon.Connection))
                //        {
                //            MessageBox.Show("Service type successfully added!");

                //            //clearing textboxes
                //            serviceName.Clear();
                //            serviceDesc.Clear();
                //            servicePrice.Value = 0;
                //            MainVM.Ldt.worker.RunWorkerAsync();
                //        }
                //        MainVM.isEdit = false;
                //    }
                //    else
                //    {
                //        string query = "UPDATE `services_T` SET serviceName = '" + serviceName.Text + "',serviceDesc = '" + serviceDesc.Text + "', servicePrice = '" + servicePrice.Value + "' WHERE serviceID = '" + MainVM.SelectedService.ServiceID + "'";
                //        if (dbCon.insertQuery(query, dbCon.Connection))
                //        {
                //            //MessageBox.Show("Sevice type sucessfully updated");
                //            MainVM.Ldt.worker.RunWorkerAsync();
                //            OnSaveCloseButtonClicked(e);
                //        }
                //        MainVM.isEdit = false;
                //    }
                //}
                //else
                //    MessageBox.Show("Resolve the error first");
                //validationError = false;
                MainVM.SelectedService.PhaseGroups.Add(new PhaseGroup() { PhaseGroupName = phaseNameTb.Text, PhaseGroupDesc = phaseDescTb.Text, PhaseItems = MainVM.SelectedPhaseGroup.PhaseItems });
            }
            else if (result == MessageBoxResult.Cancel)
            {
            }
            
            
            OnSaveCloseButtonClicked(e);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCloseButtonClicked(e);
        }

        private void moveUpSeqNoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MainVM.SelectedPhaseGroup.PhaseItems.Count > 2)
            {
                MainVM.SelectedPhase.FirstItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) + 1);
            }
            else if ((MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1) == MainVM.SelectedPhaseGroup.PhaseItems.Count - 2)
            {

                MainVM.SelectedPhase.LastItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.Count - 2].FirstItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 1);
            }
            else if (MainVM.SelectedPhaseGroup.PhaseItems.Count == 2)
            {
                MainVM.SelectedPhase.FirstItem = true;
                MainVM.SelectedPhase.LastItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.Count - 2].FirstItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) - 2);
            }
            
        }

        private void moveDownSeqNoBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MainVM.SelectedPhaseGroup.PhaseItems.Count > 2)
            {
                MainVM.SelectedPhase.FirstItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) + 1);
            }
            else if ((MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase)+1) == MainVM.SelectedPhaseGroup.PhaseItems.Count - 1)
            {
                MainVM.SelectedPhase.LastItem = true;
                MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.Count - 1].LastItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) + 1);
            }
            else if(MainVM.SelectedPhaseGroup.PhaseItems.Count == 2)
            {
                MainVM.SelectedPhase.FirstItem = false;
                MainVM.SelectedPhase.LastItem = true;
                MainVM.SelectedPhaseGroup.PhaseItems[MainVM.SelectedPhaseGroup.PhaseItems.Count - 1].LastItem = false;
                MainVM.SelectedPhaseGroup.PhaseItems.Move(MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase), MainVM.SelectedPhaseGroup.PhaseItems.IndexOf(MainVM.SelectedPhase) + 1);
            }
        }
    }
}
